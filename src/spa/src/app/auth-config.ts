import { LogLevel, Configuration, BrowserCacheLocation } from "@azure/msal-browser";

const isIE = window.navigator.userAgent.indexOf("MSIE") > -1 || window.navigator.userAgent.indexOf("Trident/") > -1;

export const b2cPolicies = {
    names: {
        signUpSignIn: "B2C_1_SI",
        editProfile: "B2C_1_EDIT_PROFILE"
    },
    authorities: {
        signUpSignIn: {
            authority: "https://sgpoc.b2clogin.com/sgpoc.onmicrosoft.com/B2C_1A_SIGNIN_ONLY",
        },
        editProfile: {
            authority: "https://sgpoc.b2clogin.com/sgpoc.onmicrosoft.com/b2c_1_edit_profile"
        }
    },
    authorityDomain: "sgpoc.b2clogin.com"
};

export const msalConfig: Configuration = {
    auth: {
        clientId: '295a1e5c-12e9-4059-b3ae-4595824f5127', // This is the ONLY mandatory field that you need to supply.
        authority: b2cPolicies.authorities.signUpSignIn.authority, // Defaults to "https://login.microsoftonline.com/common"
        knownAuthorities: [b2cPolicies.authorityDomain], // Mark your B2C tenant's domain as trusted.
        redirectUri: '/', // Points to window.location.origin. You must register this URI on Azure portal/App Registration.
        postLogoutRedirectUri: '/', // Indicates the page to navigate after logout.
        navigateToLoginRequestUrl: true, // If "true", will navigate back to the original request location before processing the auth code response.
    },
    cache: {
        cacheLocation: BrowserCacheLocation.LocalStorage, // Configures cache location. "sessionStorage" is more secure, but "localStorage" gives you SSO between tabs.
        storeAuthStateInCookie: isIE, // Set this to "true" if you are having issues on IE11 or Edge
    },
    system: {
        loggerOptions: {
            loggerCallback(logLevel: LogLevel, message: string) {
                console.log(message);
            },
            logLevel: LogLevel.Verbose,
            piiLoggingEnabled: false
        }
    }
}

export const protectedResources = {
    groupsApi: {
        endpoint: "https://b2capimanagement.azurewebsites.net/api/groups",
        // endpoint: "https://localhost:7182/api/groups",
        scopes: ["https://sgpoc.onmicrosoft.com/api/api.read"],
    }
}

export const loginRequest = {
    scopes: []
}