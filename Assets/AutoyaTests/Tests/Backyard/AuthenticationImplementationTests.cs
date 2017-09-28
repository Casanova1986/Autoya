// using System;
// using System.Collections;
// using System.IO;
// using AutoyaFramework;
// using AutoyaFramework.Settings.Auth;
// using Miyamasu;
// using UnityEngine;

// /**
// 	test for authorization flow control.
// */
// public class AuthImplementationTests : MiyamasuTestRunner {
// 	private void DeleteAllData (string path) {
// 		if (Directory.Exists(path)) {
// 			Directory.Delete(path, true);
// 		}
// 	}
	
// 	[MSetup] public IEnumerator Setup () {
// 		Autoya.ResetAllForceSetting();

// 		var authorized = false;
// 		var dataPath = Application.persistentDataPath;

// 		var fwPath = Path.Combine(dataPath, AuthSettings.AUTH_STORED_FRAMEWORK_DOMAIN);
// 		DeleteAllData(fwPath);

// 		Autoya.TestEntryPoint(dataPath);

// 		Autoya.Auth_SetOnAuthenticated(
// 			() => {
// 				authorized = true;
// 			}
// 		);

// 		yield return WaitUntil(
// 			() => {
// 				return authorized;
// 			},
// 			() => {throw new TimeoutException("timeout in setup.");},
// 			10
// 		);

// 		True(Autoya.Auth_IsAuthenticated(), "not logged in.");
// 	}

// 	[MTeardown] public IEnumerator Teardown () {
// 		Autoya.Shutdown();
// 		Autoya.ResetAllForceSetting();
// 		while (GameObject.Find("AutoyaMainthreadDispatcher") != null) {
// 			yield return null;
// 		}
// 	}

	
// 	[MTest] public IEnumerator WaitDefaultAuthenticate () {
// 		True(Autoya.Auth_IsAuthenticated(), "not yet logged in.");
// 		yield break;
// 	}

// 	[MTest] public IEnumerator DeleteAllUserData () {
// 		Autoya.Auth_DeleteAllUserData();
		
// 		var authenticated = Autoya.Auth_IsAuthenticated();
// 		True(!authenticated, "not deleted.");

// 		Autoya.Auth_AttemptAuthentication();
		
// 		yield return WaitUntil(
// 			() => Autoya.Auth_IsAuthenticated(),
// 			() => {throw new TimeoutException("failed to firstBoot.");}
// 		);
// 	}

// 	[MTest] public IEnumerator HandleBootAuthFailed () {
// 		Autoya.forceFailFirstBoot = true;

// 		Autoya.Auth_DeleteAllUserData();
		
// 		var bootAuthFailHandled = false;
// 		Autoya.Auth_SetOnBootAuthFailed(
// 			(code, reason) => {
// 				bootAuthFailHandled = true;
// 			}
// 		);

// 		Autoya.Auth_AttemptAuthentication();
		
// 		yield return WaitUntil(
// 			() => bootAuthFailHandled,
// 			() => {throw new TimeoutException("failed to handle bootAuthFailed.");},
// 			10
// 		);
		
// 		Autoya.forceFailFirstBoot = false;
// 	}

// 	[MTest] public IEnumerator HandleBootAuthFailedThenAttemptAuthentication () {
// 		Autoya.forceFailFirstBoot = true;

// 		Autoya.Auth_DeleteAllUserData();
		
// 		var bootAuthFailHandled = false;
// 		Autoya.Auth_SetOnBootAuthFailed(
// 			(code, reason) => {
// 				bootAuthFailHandled = true;
// 			}
// 		);
		
// 		Autoya.Auth_AttemptAuthentication();
		
// 		yield return WaitUntil(
// 			() => bootAuthFailHandled,
// 			() => {throw new TimeoutException("failed to handle bootAuthFailed.");},
// 			10
// 		);
		
// 		Autoya.forceFailFirstBoot = false;

// 		Autoya.Auth_AttemptAuthentication();
		
// 		yield return WaitUntil(
// 			() => Autoya.Auth_IsAuthenticated(),
// 			() => {throw new TimeoutException("failed to attempt auth.");}
// 		);
// 	}
	
// 	[MTest] public IEnumerator HandleLogoutThenAuthenticationAttemptSucceeded () {
// 		Autoya.Auth_Logout();

// 		Autoya.Auth_AttemptAuthentication();

// 		yield return WaitUntil(
// 			() => Autoya.Auth_IsAuthenticated(),
// 			() => {throw new TimeoutException("failed to auth");}
// 		);
// 	}

	
// 	[MTest] public IEnumerator IntentionalLogout () {
// 		Autoya.Auth_Logout();
		
// 		var loggedIn = Autoya.Auth_IsAuthenticated();
// 		True(!loggedIn, "state does not match.");
// 		yield break;
// 	}

// 	[MTest] public IEnumerator HandleTokenRefreshFailed () {
// 		Autoya.forceFailTokenRefresh = true;
		
// 		var tokenRefreshFailed = false;
// 		Autoya.Auth_SetOnRefreshAuthFailed(
// 			(code, reason) => {
// 				tokenRefreshFailed = true;
// 			}
// 		);

// 		// forcibly get 401 response.
// 		Autoya.Http_Get(
// 			"https://httpbin.org/status/401", 
// 			(conId, resultData) => {
// 				// do nothing.
// 			},
// 			(conId, code, reason, autoyaStatus) => {
// 				// do nothing.
// 			}
// 		);

// 		yield return WaitUntil(
// 			() => tokenRefreshFailed,
// 			() => {throw new TimeoutException("failed to handle tokenRefreshFailed.");},
// 			10
// 		);
		
// 		Autoya.forceFailTokenRefresh = false;
// 	}

// 	[MTest] public IEnumerator HandleTokenRefreshFailedThenAttemptAuthentication () {
// 		Autoya.forceFailTokenRefresh = true;
		
// 		var tokenRefreshFailed = false;
// 		Autoya.Auth_SetOnRefreshAuthFailed(
// 			(code, reason) => {
// 				tokenRefreshFailed = true;
// 			}
// 		);
		
// 		// forcibly get 401 response.
// 		Autoya.Http_Get(
// 			"https://httpbin.org/status/401", 
// 			(conId, resultData) => {
// 				// do nothing.
// 			},
// 			(conId, code, reason, autoyaStatus) => {
// 				// do nothing.
// 			}
// 		);

// 		yield return WaitUntil(
// 			() => tokenRefreshFailed,
// 			() => {throw new TimeoutException("failed to handle tokenRefreshFailed.");},
// 			10
// 		);
		
// 		Autoya.forceFailTokenRefresh = false;
		
// 		Autoya.Auth_AttemptAuthentication();
		
// 		yield return WaitUntil(
// 			() => Autoya.Auth_IsAuthenticated(),
// 			() => {throw new TimeoutException("failed to handle tokenRefreshFailed.");},
// 			15
// 		);
// 	}

//     [MTest] public IEnumerator UnauthorizedThenHttpGet () {
// 		var reauthenticationSucceeded = false;

// 		// forcibly get 401 response.
// 		Autoya.Http_Get(
// 			"https://httpbin.org/status/401",
// 			(conId, resultData) => {
// 				// do nothing.
// 			},
// 			(conId, code, reason, autoyaStatus) => {
//                 // these handler will be fired automatically.
// 				Autoya.Auth_SetOnAuthenticated(
//                     () => {
//                         Autoya.Http_Get(
//                             "https://httpbin.org/get",
//                             (string conId2, string data2) => {
//                                 reauthenticationSucceeded = true;
//                             },
//                             (conId2, code2, reason2, autoyaStatus2) => {
//                                 // do nothing.
//                             }
//                         );
//                     }
//                 );
// 			}
// 		);

// 		yield return WaitUntil(
// 			() => reauthenticationSucceeded,
// 			() => {throw new TimeoutException("failed to handle SetOnAuthenticated.");},
// 			10
// 		);
// 	}
// }