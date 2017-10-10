
using System;
using System.Collections.Generic;
using System.Linq;
using AutoyaFramework.AppManifest;
using UnityEngine;

namespace AutoyaFramework {
	public partial class Autoya {
        private AppManifestStore<RuntimeManifestObject, BuildManifestObject> manifestStore;

        private void InitializeAppManifest () {
            manifestStore = new AppManifestStore<RuntimeManifestObject, BuildManifestObject>(OnOverwriteRuntimeManifest, OnLoadRuntimeManifest);
        }

        /*
            public functions
         */
        public static Dictionary<string, string> Manifest_GetAppManifest () {
            return autoya.manifestStore.GetParamDict();
        }

        public static bool Manifest_UpdateRuntimeManifest (RuntimeManifestObject updated) {
            return autoya.manifestStore.UpdateRuntimeManifest(updated);
        }

        public static RuntimeManifestObject Manifest_LoadRuntimeManifest () {
            return autoya.manifestStore.GetRuntimeManifest();
        }

        /*
            editor functions
         */
        #if UNITY_EDITOR
        [UnityEditor.InitializeOnLoad] public class BuildEntryPoint {
            // ${UNITY_APP} -batchmode -projectPath $(pwd) -quit -executeMethod AutoyaFramework.BuildEntryPoint.Entry -m "herecomes!"

            static BuildEntryPoint () {
                var buildMessage = string.Empty;
                
                var commandLineOptions = System.Environment.GetCommandLineArgs().ToList();
                if (commandLineOptions.Contains("-batchmode")) {
                    for (var i = 0; i < commandLineOptions.Count; i++) {
                        var param = commandLineOptions[i];
                        
                        switch (param) {
                            case "-m":
                            case "--message": {
                                if (i < commandLineOptions.Count - 1) {
                                    buildMessage = commandLineOptions[i+1];
                                }
                                break;
                            }

                        }
                    }
                }
                
                if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) {
                    OnCompile(buildMessage);
                }
            }

            public static void Entry () {
                // 何にもすることがないが、コマンドライン処理をすることはできる。でもまあ、だいたい独自になんかすると思うので、出しゃばる必要はない気がする。
                // 理想的な挙動について考えよう。
            }

            private static void OnCompile (string buildMessage=null) {
                AppManifestStore<RuntimeManifestObject, BuildManifestObject>.UpdateBuildManifest(
                    current => {
                        // countup build count.
                        var buildNoStr = current.buildNo;
                        var buildNoNum = Convert.ToInt64(buildNoStr) + 1;
                        current.buildNo = buildNoNum.ToString();

                        // set message if exist.
                        if (!string.IsNullOrEmpty(buildMessage)) {
                            current.buildMessage = buildMessage;  
                        }

                        current.buildDate = DateTime.Now.ToString();

                        return current;
                    }
                );
            }
        }
        #endif
    }
}