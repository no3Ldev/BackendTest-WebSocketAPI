# BackendTest-WebAPI
(Sidekick) Backend Test - Web API

ASP.NET Core 5.0 Web API <br>
Configured for HTTPS <br>
Enabled Docker Support (Windows) <br>
Enabled OpenAPI support <br>

![image](https://user-images.githubusercontent.com/13361597/111087768-5ede9e80-855e-11eb-857e-6497c8b76a6a.png)

PROJECTS: <br>
BackendTest-WebAPI - main web api project (VS > Debug > Start Debugging) <br>
BackendTest-UnitTest - unit testing project to test web api functions (VS > Test > Run All Test) <br>

BackendTest-UnitTest: <br>
_BaseClass - shared methods used among all test classes <br>
1-5*Test.cs - unit testing methods for web api <br>
6-WebSocketTest - unresolved. unit testing for web socket <br>
6-WebSocketTest.html - unit testing for web socket using javascript (Open on web browser to run) <br>

![image](https://user-images.githubusercontent.com/13361597/111085870-5c774700-8554-11eb-9d26-a9288265113c.png)
![image](https://user-images.githubusercontent.com/13361597/111085925-96e0e400-8554-11eb-8611-7ee9bbd5575d.png)

CONTROLLERS: <br>
AuthenticateController - generate login salt <br>
AvailableController - checks if username or email has already been used <br>
BaseController - shared methods across all controllers <br>
HashController - generate sha 256 hmac hash of text and key <br>
LoginController - login user and details <br>
RegisterController - register new user to database <br>
VerifyController - generate verification code <br>

SERVICES: <br>
WebSocketExtension - add middleware as service to project <br>
WebSockHandler - socket connection and events handler <br>
WebSocketmanager - sockets connections manager <br>
WebSocketMiddleware - implements sockets management and events <br>
WebSocketRequestHandler - handles requests and response to/from clients <br>

TABLES: <br>
User - where registered users are stored <br>
Authentication - stores login salt requests and validity <br>
Login - successful logins history <br>
Verification - stores verification codes and validity <br>

APPSETTINGS: <br>
base_address - actual url path to web api <br>
https_port - port number for https connections <br>
superSecretKey - secret key to further hash stored passwords <br>
salt_expiry - login salt expiration in seconds <br>
session_expiry - login session expiration in seconds <br>
verification_expiry - verification code expiration in seconds <br>
verification_max - maximum verification code requests per day <br>
