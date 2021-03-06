
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AutoyaFramework.AppManifest
{
    /**
        Autoyaで使用する動的な設定パラメータに関する型情報。
        アプリケーション内に保存される。

        動的に書き換えることができる。
        初期値を与えることができる。

        独自型を置く場合、ToString()をつけると、Autoya.Manifest_GetAppManifest()メソッドでSerializeされた情報を表示できる。
    */
    [Serializable]
    public class RuntimeManifestObject
    {
        [SerializeField]
        public AssetBundleListInfo[] resourceInfos;

        public RuntimeManifestObject()
        {
            resourceInfos = new AssetBundleListInfo[]{
               new AssetBundleListInfo
               {
                   listIdentity = "main_assets",
                   listVersion = "1.0.0",
                   listDownloadUrl = "https://raw.githubusercontent.com/sassembla/Autoya/master/AssetBundles"
               },
               new AssetBundleListInfo
               {
                   listIdentity = "sub_assets",
                   listVersion = "1.0.0",
                   listDownloadUrl = "https://raw.githubusercontent.com/sassembla/Autoya/master/AssetBundles"
               },
               new AssetBundleListInfo
               {
                    listIdentity = "scenes",
                   listVersion = "1.0.0",
                   listDownloadUrl = "https://raw.githubusercontent.com/sassembla/Autoya/master/AssetBundles"
               }
           };
        }

        public override string ToString()
        {
            return "AssetBundleListInfos:" + string.Join(",\n", resourceInfos.Select(item => "listIdentity:" + item.listIdentity + " listDownloadUrl:" + item.listDownloadUrl + " listVersion:" + item.listVersion).ToArray());
        }
    }

    [Serializable]
    public class AssetBundleListInfo
    {
        [SerializeField] public string listIdentity;
        [SerializeField] public string listDownloadUrl;
        [SerializeField] public string listVersion;
    }
}
