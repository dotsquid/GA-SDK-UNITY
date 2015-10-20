﻿#if !UNITY_4_6 // For getting this to work in Unity 4.6 read the iOS build section on our wiki on github at https://github.com/GameAnalytics/GA-SDK-UNITY/wiki/iOS%20Build

using UnityEditor.iOS.Xcode;
using UnityEditor.Callbacks;
using UnityEditor;
using System.IO;

namespace GameAnalyticsSDK
{
	public class GA_PostprocessBuild
	{
		[PostProcessBuild]
		public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
		{
			#if UNITY_4_6
			if (buildTarget == BuildTarget.iPhone)
			#else
			if (buildTarget == BuildTarget.iOS)
			#endif
			{
				string projPath = PBXProject.GetPBXProjectPath(path);
				
				PBXProject proj = new PBXProject();
				proj.ReadFromString(File.ReadAllText(projPath));
				
				string targetName = PBXProject.GetUnityTargetName();
				string target = proj.TargetGuidByName(targetName);
				
				proj.AddFileToBuild(target, proj.AddFile("usr/lib/libsqlite3.dylib", "Frameworks/libsqlite3.dylib", PBXSourceTree.Sdk));
				proj.AddFileToBuild(target, proj.AddFile("usr/lib/libz.dylib", "Frameworks/libz.dylib", PBXSourceTree.Sdk));
				proj.AddFileToBuild(target, proj.AddFile("Frameworks/AdSupport.framework", "Frameworks/AdSupport.framework", PBXSourceTree.Sdk));
				
				File.WriteAllText(projPath, proj.WriteToString());
			}
		}
	}
}

#endif
