using BepInEx;
using HarmonyLib;
using System;
using System.IO;
using UnityEngine;

namespace rainbowMilla
{
    [BepInPlugin("com.admiraldock.rainbowMilla", MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            var harmony = new Harmony("com.admiraldock.rainbowMilla");
            harmony.PatchAll(typeof(millaTexturePatch));
            harmony.PatchAll(typeof(millaUpdateColors));
        }
    }
    class millaTexturePatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(FPPlayer), "Start", MethodType.Normal)]
        static void Postfix()
        {
            if (GameObject.Find("Player 1").GetComponent<FPPlayer>().characterID.ToString() == "MILLA")
            {
                SpriteRenderer millaRenderer = (SpriteRenderer)GameObject.Find("Player 1").GetComponent(typeof(SpriteRenderer));
                Texture2D millaTexture = millaRenderer.sprite.texture;
                string millaTexturePath = Path.Combine(Path.GetFullPath("."), "mod_overrides");
                if (File.Exists(millaTexturePath + "\\SpriteAtlasTexture-Milla-1024x2048-fmt4.png"))
                {
                    millaTexture.LoadImage(File.ReadAllBytes(millaTexturePath + "\\SpriteAtlasTexture-Milla-1024x2048-fmt4.png"));
                }
            }
        }
    }
    class millaUpdateColors
    {
        private static float update = 0f;
        private static double r = 1, g = 0, b = 0, a = 1;
        private static SpriteRenderer millaRenderer = (SpriteRenderer)GameObject.Find("Player 1").GetComponent(typeof(SpriteRenderer));

        [HarmonyPostfix]
        [HarmonyPatch(typeof(FPPlayer), "Update", MethodType.Normal)]
        static void PostFix(FPPlayer __instance)
        {
            update += FPStage.deltaTime;
            if (update > 1.0f)
            {
                if (__instance.characterID.ToString() == "MILLA")
                {
                    r = Math.Round(r, 2);
                    b = Math.Round(b, 2);
                    g = Math.Round(g, 2);
                    //millaRenderer.color = new Color((float)r, (float)g, (float)b, (float)a);
                    if (__instance.childSprite.childSprite != null)
                    {
                        __instance.childSprite.childSprite.color = new Color((float)r, (float)g, (float)b, (float)a);
                    }
                    if (__instance.GetComponent<SpriteRenderer>() != null)
                    {
                        __instance.GetComponent<SpriteRenderer>().color = new Color((float)r, (float)g, (float)b, (float)a);
                    }
                    //FileLog.Log("Values " + r + " " + g + " " + b);
                    if (r <= 1 && b < 1 && g <= 0)
                    {
                        r = r - 0.01;
                        b = b + 0.01;
                        update = 0.0f;
                        return;
                    }
                    if (b <= 1 && g < 1 && r <= 0)
                    {
                        b = b - 0.01;
                        g = g + 0.01;
                        update = 0.0f;
                        return;
                    }    
                    if (g <= 1 && r < 1 && b <= 0)
                    {
                        g = g - 0.01;
                        r = r + 0.01;
                        update = 0.0f;
                        return;
                    }
                }
                update = 0.0f;
            }
        }
    }
}