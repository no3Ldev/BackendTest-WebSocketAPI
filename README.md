# Backend Test - WebSocket API Endpoint
This is a sample ASP.NET Core WebSocket project that exposes both raw WebSockets endpoints to process API requests in JSON format from client. It uses Microsoft Azure Database for PostgreSQL as its datasource and Entity Framework Core for its data operations such as user registration and authentication, session management, and verification code system.

## Technologies Used:
* ASP.NET Core 5.0 Web API
* Configured for HTTPS
* Enabled Docker Support (Windows)
* Raw WebSocket protocol
* MSTest Unit Testing
* Azure Database for PostgreSQL v11
* Microsoft Entity Framework Core (PostgreSQL)
* SHA-256 HMAC hashing

## Visual Studio Projects
* <b>BackendTest-WebSocket</b> - main web socket project
* <b>BackendTest-UnitTest</b> - unit testing project to test class functions

## Unit-Test Project: BackendTest-UnitTest
* <b>_BaseClass.cs</b> - shared methods used among all test classes
* <b>1-5*Test.cs</b> - unit testing for class functions
* <b>6-WebSocketTest.html</b> - testing for web socket using javascript

## How to Test?
* <b>MSTest Unit-Testing for API functions</b> - on visual studio, press `Ctrl+R, A` or go to `Test` menu, then click `Run All Test` item
* <b>Web Client for Web Socket</b> - run `BackendTest-WebSocket` project and open `BackEndTest-UnitTest\6-WebSocketTest.html` on your web browser
* <b>Use Third-Party WebSocket Test Client for Chrome</b> - install `WebSocket Test Client` extension found <a href="https://chrome.google.com/webstore/detail/websocket-test-client/fgponpodhbmadfljofbimhhlengambbn" target="_blank">here</a> in Chrome Web Store

## Sample JSON Request and Response
``` json
/* Client Request */
{
    "command": "emailVerification",
    "email": "john.doe@mail.com",
    "username": "johndoe"
}
/* Server Response */
{
    "command": "emailVerification",
    "success": true,
    "remarks": null
}
/* Remarks will contain reason when success returns false */
```

## Web API Controllers: <br>
* <b>AuthenticateController.cs</b> - generate login salt <br>
* <b>AvailableController.cs</b> - checks if username or email has already been used <br>
* <b>BaseController.cs</b> - shared methods across all controllers <br>
* <b>HashController.cs</b> - generate sha 256 hmac hash of text and key <br>
* <b>LoginController.cs</b> - login user and details <br>
* <b>RegisterController.cs</b> - register new user to database <br>
* <b>VerifyController.cs</b> - generate verification code <br>

## Middleware Service
* <b>WebSocketExtension.cs</b> - add middleware as service to project <br>
* <b>WebSockHandler.cs</b> - socket connection and events handler <br>
* <b>WebSocketmanager.cs</b> - sockets connections manager <br>
* <b>WebSocketMiddleware.cs</b> - implements sockets management and events <br>
* <b>WebSocketRequestHandler.cs</b> - handles requests and response to/from clients <br>

## Database Tables
* <b>User</b> - where registered users are stored
* <b>Authentication</b> - stores login salt requests and validity
* <b>Login</b> - successful logins history
* <b>Verification</b> - stores verification codes and validity

## Configuration: AppSettings
* <b>https_port</b> - port number for https connections <br>
* <b>superSecretKey</b> - secret key to further hash stored passwords <br>
* <b>salt_expiry</b> - login salt expiration in seconds <br>
* <b>session_expiry</b> - login session expiration in seconds <br>
* <b>verification_expiry</b> - verification code expiration in seconds <br>
* <b>verification_max</b> - maximum verification code requests per day <br>

## Testing Screenshots:
![image](https://user-images.githubusercontent.com/13361597/111085870-5c774700-8554-11eb-9d26-a9288265113c.png)
![image](https://user-images.githubusercontent.com/13361597/111085925-96e0e400-8554-11eb-8611-7ee9bbd5575d.png)

