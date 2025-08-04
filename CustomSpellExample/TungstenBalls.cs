using BepInEx;
using UnityEngine;
using System.Threading.Tasks;
using BlackMagicAPI.Enums;
using BlackMagicAPI.Modules.Spells;
using BlackMagicAPI.Helpers;
using BlackMagicAPI.Managers;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Steamworks;
namespace TungstenBalls
{
    [BepInPlugin("com.magearena.tungstenballs", "Tungsten Balls", "1.0.0")]
    [BepInProcess("MageArena.exe")]
    [BepInDependency("com.magearena.modsync", BepInDependency.DependencyFlags.HardDependency)]
    public class TungstenBalls : BaseUnityPlugin
    {
        // This mod requires both client and host to have it
        public static string modsync = "all";
        
        // Cache the AssetBundle and audio clip for efficient loading
        private static AssetBundle tungstenBallsBundle = null;
        public static AudioClip dongSound = null;
        
        private void Awake()
        {
            Logger.LogInfo("ModSync found! Initializing Tungsten Balls...");
            
            // Load the AssetBundle at startup
            LoadTungstenBallsAssets();
            
            SpellManager.RegisterSpell(this, typeof(TungstenBallsSpellData), typeof(TungstenBallsSpellLogic));
        }
        
        private void LoadTungstenBallsAssets()
        {
            try
            {
                // Get the plugin's directory path dynamically
                string pluginPath = System.IO.Path.GetDirectoryName(Info.Location);
                string assetBundlePath = System.IO.Path.Combine(pluginPath, "Assets", "tungstenballls");
                
                if (System.IO.File.Exists(assetBundlePath))
                {
                    tungstenBallsBundle = AssetBundle.LoadFromFile(assetBundlePath);
                    
                    if (tungstenBallsBundle != null)
                    {
                        // Load the audio clip from the bundle
                        dongSound = tungstenBallsBundle.LoadAsset<AudioClip>("dong");
                        
                        if (dongSound == null)
                        {
                            Debug.LogWarning("[TungstenBalls] Could not find 'dong' audio clip in AssetBundle");
                        }
                    }
                    else
                    {
                        Debug.LogError("[TungstenBalls] Failed to load AssetBundle");
                    }
                }
                else
                {
                    Debug.LogError($"[TungstenBalls] AssetBundle file does not exist at path: {assetBundlePath}");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[TungstenBalls] Error loading AssetBundle: {e.Message}");
            }
        }
        

    }

    internal class TungstenBallsSpellData : SpellData
    {
        public override SpellType SpellType => SpellType.Page; // Or other types
        public override string Name => "Tungsten Balls";
        public override float Cooldown => 20f; // Cooldown in seconds
        public override Color GlowColor => new Color(0.8f, 0.6f, 0.5f); // Custom skin tone color
    }

    internal class TungstenBallsSpellLogic : SpellLogic
    {
        // Track which players are currently under the Tungsten Balls effect
        public static Dictionary<PlayerMovement, bool> affectedPlayers = new Dictionary<PlayerMovement, bool>();
        
        public override void CastSpell(GameObject playerObj, Vector3 direction, int level)
        {
            // Allow spell to be processed on all clients
            PlayerMovement casterMovement = playerObj.GetComponent<PlayerMovement>();
            if (casterMovement == null)
            {
                return;
            }
            
            // Find all players within 30 meters in a cone of vision
            GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");
            Vector3 casterPosition = playerObj.transform.position;
            Vector3 casterForward = playerObj.transform.forward;
            
            // Get local player's Steam name for protection
            string localSteamName = SteamFriends.GetPersonaName();
            
            // Find the local player by matching Steam name
            PlayerMovement localPlayer = null;
            PlayerMovement[] allPlayerMovements = Object.FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None);
            foreach (PlayerMovement pm in allPlayerMovements)
            {
                if (pm.playername == localSteamName)
                {
                    localPlayer = pm;
                    break;
                }
            }
            
                         // Find the best valid target (prioritize crosshair proximity, then distance)
             GameObject bestTarget = null;
             float bestScore = float.MaxValue; // Lower score = better target
             PlayerMovement bestTargetMovement = null;
            
            foreach (GameObject targetPlayer in allPlayers)
            {
                if (targetPlayer == playerObj) continue; // Skip self
                
                Vector3 directionToTarget = (targetPlayer.transform.position - casterPosition).normalized;
                float distanceToTarget = Vector3.Distance(casterPosition, targetPlayer.transform.position);
                
                // Check if target is within 30 meters
                if (distanceToTarget <= 30f)
                {
                    // Check if target is in cone of vision (45 degree angle)
                    float angle = Vector3.Angle(casterForward, directionToTarget);
                    if (angle <= 45f)
                    {
                        // Check team targeting - skip teammates (same team as caster)
                        PlayerMovement targetMovement = targetPlayer.GetComponent<PlayerMovement>();
                        if (targetMovement != null)
                        {
                            // Check if target is on the same team as caster
                            if (casterMovement.playerTeam == targetMovement.playerTeam)
                            {
                                continue; // Skip teammates
                            }
                        }
                        
                                                 // Use 3-raycast fan pattern to check if target is visible (line of sight) - ALL clients use caster's perspective
                         Vector3 raycastStart = casterPosition + Vector3.up * 1.5f; // Start from caster's head level
                         Vector3 targetPosition = targetPlayer.transform.position + Vector3.up * 1.5f; // Aim at target's head level
                         Vector3 baseDirection = (targetPosition - raycastStart).normalized;
                         float raycastDistance = Vector3.Distance(raycastStart, targetPosition);
                         
                         // Layer mask to ignore players (we want to hit walls/obstacles, not other players)
                         int layerMask = ~(1 << LayerMask.NameToLayer("Player"));
                         
                                                   // Fan pattern angles (in degrees) - tight spread to catch player's head even with partial cover
                          float[] fanAngles = { -5f, 0f, 5f }; // Left, center, right (tight 10° spread)
                          bool anyRaycastClear = false;
                          
                          // Perform 3 raycasts in a horizontal fan pattern from the same starting point
                          for (int i = 0; i < fanAngles.Length; i++)
                          {
                              // Calculate rotated direction for this raycast (all start from caster's head)
                              Vector3 rotatedDirection = Quaternion.Euler(0, fanAngles[i], 0) * baseDirection;
                              
                              RaycastHit hit;
                              if (Physics.Raycast(raycastStart, rotatedDirection, out hit, raycastDistance, layerMask))
                              {
                                  // Hit something between caster and target area for this raycast
                              }
                              else
                              {
                                  // This raycast is clear - target area is visible from this angle
                                  anyRaycastClear = true;
                                  break; // If any raycast is clear, the target is visible
                              }
                          }
                         
                                                                            if (anyRaycastClear)
                          {
                              // At least one raycast is clear - target is visible! Calculate targeting score
                              
                              // Check if they're already affected (for local player protection)
                              if (targetMovement != null)
                              {
                                  // Check if this is the local player and if they're already under the effect
                                  if (localPlayer != null && targetMovement == localPlayer)
                                  {
                                      if (affectedPlayers.ContainsKey(targetMovement) && affectedPlayers[targetMovement])
                                      {
                                          continue; // Skip local player if already affected
                                      }
                                  }
                                  
                                  // Calculate targeting score (prioritize crosshair proximity, then distance)
                                  float crosshairAngle = Vector3.Angle(casterForward, directionToTarget);
                                  float targetingScore = crosshairAngle * 2f + distanceToTarget; // Crosshair angle weighted more heavily
                                  
                                  // Check if this is the best target so far
                                  if (targetingScore < bestScore)
                                  {
                                      // This is a valid target - update best target
                                      bestTarget = targetPlayer;
                                      bestScore = targetingScore;
                                      bestTargetMovement = targetMovement;
                                  }
                              }
                          }
                    }
                }
            }
            
                                      // Apply effect to the best valid target
             if (bestTarget != null && bestTargetMovement != null)
             {
                 // Mark player as affected
                 affectedPlayers[bestTargetMovement] = true;
                 
                 // Use reflection to call the private FrogRpc method
                 MethodInfo frogRpcMethod = typeof(PlayerMovement).GetMethod("FrogRpc", BindingFlags.NonPublic | BindingFlags.Instance);
                 if (frogRpcMethod != null)
                 {
                     frogRpcMethod.Invoke(bestTargetMovement, null);
                 }
                 
                 // Start the fall effect coroutine
                 bestTargetMovement.StartCoroutine(TungstenBallsFallEffect(bestTargetMovement));
             }
        }
        
        // FIXED: Made this method public static so it can be accessed from other classes
        public static IEnumerator TungstenBallsFallEffect(PlayerMovement targetMovement)
        {
            // Store original values
            bool originalCanMove = targetMovement.canMove;
            bool originalCanMoveCamera = targetMovement.canMoveCamera;
            bool originalEnabled = targetMovement.enabled;
            
            // Disable movement and camera control
            targetMovement.canMove = false;
            targetMovement.canMoveCamera = false;
            
            // Play the "dong" sound when they hit the ground
            PlayTungstenBallsSound(targetMovement.transform.position);
            
            // === DAMAGE THE VICTIM ===
            // Wait 0.5 seconds before applying damage
            yield return new WaitForSeconds(0.5f);
            
                         // Apply 10 damage to the victim
             targetMovement.NonRpcDamagePlayer(10f, null, "tungstenballs");
            
            // === LOCAL PLAYER MODEL EFFECTS ===
            // Find the player's model and UI elements
            GameObject playerRoot = targetMovement.gameObject;
            Transform wizardTrio = playerRoot.transform.Find("wizardtrio");
            
            // Store original shadow casting modes for all player models
            UnityEngine.Rendering.ShadowCastingMode[] originalShadowModes = null;
            SkinnedMeshRenderer[] playerRenderers = null;
            
            if (wizardTrio != null)
            {
                // Find all player model renderers (Plane.002, Plane.003, Plane.004)
                playerRenderers = wizardTrio.GetComponentsInChildren<SkinnedMeshRenderer>();
                if (playerRenderers.Length > 0)
                {
                    originalShadowModes = new UnityEngine.Rendering.ShadowCastingMode[playerRenderers.Length];
                    
                                         // Store original shadow modes and set to "On"
                     for (int i = 0; i < playerRenderers.Length; i++)
                     {
                         originalShadowModes[i] = playerRenderers[i].shadowCastingMode;
                         playerRenderers[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                     }
                }
            }
            
            // Deactivate UI elements
            Transform armz = playerRoot.transform.Find("armz");
            Transform pikupact = playerRoot.transform.Find("pikupact");
            
            bool armzWasActive = false;
            bool pikupactWasActive = false;
            
                         if (armz != null)
             {
                 armzWasActive = armz.gameObject.activeSelf;
                 armz.gameObject.SetActive(false);
             }
             
             if (pikupact != null)
             {
                 pikupactWasActive = pikupact.gameObject.activeSelf;
                 pikupact.gameObject.SetActive(false);
             }
            
            // Only apply camera effects if this is the local player (the victim)
            if (targetMovement.IsOwner)
            {
                // Get camera and store original state
                Camera mainCamera = Camera.main;
                if (mainCamera == null)
                {
                    Debug.LogError("[TungstenBalls] Could not find main camera");
                    yield break;
                }
                
                // Save original camera state
                Transform originalCameraParent = mainCamera.transform.parent;
                Vector3 originalCameraPosition = mainCamera.transform.position;
                Quaternion originalCameraRotation = mainCamera.transform.rotation;
                
                // Get spectate point from target player
                Transform spectatePoint = targetMovement.SpectatePoint;
                if (spectatePoint == null)
                {
                    spectatePoint = targetMovement.transform;
                }
                
                // Camera control variables for spectator mode
                float yaw = 0f;
                float pitch = 0f;
                float sensitivity = 2f;
                float distance = 5f;
                float minDistance = 2f;
                float maxDistance = 15f;
                
                // Lock cursor for mouse control
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                
                                 // Start spectator mode
                 float effectDuration = 5f;
                 float elapsedTime = 0f;
                 
                 while (elapsedTime < effectDuration)
                 {
                     // Check if player died during the effect
                     if (targetMovement.isDead)
                     {
                         break;
                     }
                    
                    // Handle mouse input for camera movement
                    float mouseX = Input.GetAxis("Mouse X") * sensitivity;
                    float mouseY = Input.GetAxis("Mouse Y") * sensitivity;
                    
                    // Update camera angles
                    yaw += mouseX;
                    pitch -= mouseY;
                    pitch = Mathf.Clamp(pitch, -80f, 80f); // Limit vertical rotation
                    
                    // Handle scroll wheel for zoom
                    float scroll = Input.GetAxis("Mouse ScrollWheel");
                    distance -= scroll * 5f; // Zoom speed
                    distance = Mathf.Clamp(distance, minDistance, maxDistance);
                    
                    // Calculate camera position
                    Vector3 targetPosition = spectatePoint.position;
                    
                    // Convert angles to radians
                    float yawRad = yaw * Mathf.Deg2Rad;
                    float pitchRad = pitch * Mathf.Deg2Rad;
                    
                    // Calculate camera offset
                    float horizontalDistance = distance * Mathf.Cos(pitchRad);
                    Vector3 offset = new Vector3(
                        Mathf.Sin(yawRad) * horizontalDistance,
                        distance * Mathf.Sin(pitchRad),
                        Mathf.Cos(yawRad) * horizontalDistance
                    );
                    
                    // Position camera
                    mainCamera.transform.position = targetPosition + offset;
                    mainCamera.transform.LookAt(targetPosition);
                    
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                
                                 // Restore cursor state to game's expected state
                 Cursor.lockState = CursorLockMode.Locked;
                 Cursor.visible = false;
                 
                 // Restore original camera state
                 mainCamera.transform.parent = originalCameraParent;
                 mainCamera.transform.position = originalCameraPosition;
                 mainCamera.transform.rotation = originalCameraRotation;
             }
             else
             {
                 // For non-local players, just wait the duration without camera effects
                 yield return new WaitForSeconds(5f);
             }
            
            // === RESTORE LOCAL PLAYER MODEL EFFECTS ===
            // Only restore effects if player didn't die
            if (!targetMovement.isDead)
            {
                                 // Restore shadow casting modes
                 if (playerRenderers != null && originalShadowModes != null)
                 {
                     for (int i = 0; i < playerRenderers.Length && i < originalShadowModes.Length; i++)
                     {
                         playerRenderers[i].shadowCastingMode = originalShadowModes[i];
                     }
                 }
                 
                 // Reactivate UI elements
                 if (armz != null && armzWasActive)
                 {
                     armz.gameObject.SetActive(true);
                 }
                 
                 if (pikupact != null && pikupactWasActive)
                 {
                     pikupact.gameObject.SetActive(true);
                 }
                
                // Restore original values
                targetMovement.canMove = originalCanMove;
                targetMovement.canMoveCamera = originalCanMoveCamera;
                targetMovement.enabled = originalEnabled;
                
                // Apply 3 seconds of slow movement using the same pattern as ice spell
                if (targetMovement.IsOwner)
                {
                                         // Apply movement debuff similar to ice spell using reflection
                     FieldInfo stewspeedboostField = typeof(PlayerMovement).GetField("stewspeedboost", BindingFlags.NonPublic | BindingFlags.Instance);
                     if (stewspeedboostField != null)
                     {
                         float currentValue = (float)stewspeedboostField.GetValue(targetMovement);
                         stewspeedboostField.SetValue(targetMovement, currentValue - 2f);
                     }
                    
                    targetMovement.StartCoroutine(SubtractTungstenBallsDebuff(3f));
                }
                
                // Use reflection to call the private UndoFrogRpc method
                MethodInfo undoFrogRpcMethod = typeof(PlayerMovement).GetMethod("UndoFrogRpc", BindingFlags.NonPublic | BindingFlags.Instance);
                if (undoFrogRpcMethod != null)
                {
                    undoFrogRpcMethod.Invoke(targetMovement, null);
                }
            }
                         else
             {
                 // Player died - skip restoration
             }
             
             // Remove player from affected list
             if (affectedPlayers.ContainsKey(targetMovement))
             {
                 affectedPlayers[targetMovement] = false;
             }
        }
        
                 // Play the Tungsten Balls sound with proximity audio
         private static void PlayTungstenBallsSound(Vector3 position)
         {
             try
             {
                 // Use the cached audio clip from startup
                 if (TungstenBalls.dongSound != null)
                 {
                     // Create an AudioSource at the position for proximity audio
                     GameObject audioObject = new GameObject("TungstenBallsAudio");
                     audioObject.transform.position = position;
                     
                     AudioSource audioSource = audioObject.AddComponent<AudioSource>();
                     
                     audioSource.clip = TungstenBalls.dongSound;
                     audioSource.volume = 0.8f;
                     audioSource.spatialBlend = 1.0f; // Full 3D audio
                     audioSource.rolloffMode = AudioRolloffMode.Linear;
                     audioSource.minDistance = 5f;
                     audioSource.maxDistance = 50f;
                     
                     audioSource.Play();
                     
                     // Destroy the audio object after the clip finishes
                     float destroyDelay = TungstenBalls.dongSound.length + 0.1f;
                     Object.Destroy(audioObject, destroyDelay);
                 }
             }
             catch (System.Exception e)
             {
                 Debug.LogError($"[TungstenBalls] Error playing sound: {e.Message}");
             }
         }
        
                 // Coroutine to remove the Tungsten Balls movement debuff after duration
         private static IEnumerator SubtractTungstenBallsDebuff(float duration)
         {
             yield return new WaitForSeconds(duration);
             
             // Restore the movement speed by adding back the debuff
             // Note: We need to use reflection to access the private stewspeedboost field
             FieldInfo stewspeedboostField = typeof(PlayerMovement).GetField("stewspeedboost", BindingFlags.NonPublic | BindingFlags.Instance);
             if (stewspeedboostField != null)
             {
                 // We need to find the player to restore their movement speed
                 // Since this is a static method, we'll need to find the player differently
                 PlayerMovement[] allPlayers = Object.FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None);
                 foreach (PlayerMovement player in allPlayers)
                 {
                     if (player.IsOwner)
                     {
                         float currentValue = (float)stewspeedboostField.GetValue(player);
                         stewspeedboostField.SetValue(player, currentValue + 2f);
                         break;
                     }
                 }
             }
         }
    }
}