# SensorManagementAp

SensorManagementApp is a webapi that helps you view, create, update and delete sensors in the database.

When you run the solution, you can see the swagger page. You can do CURD operations on Sensor entity. Additionally there is support for an api that calls the upstream platform api, and forwards it to the user.  The following are the endpoints supported in this application:

1. Get **api/platformapi/data**: calls the upstream platform api forwards the data to the user. Incase request to platform api fails, it forwards the exact response code to the user. In case of exception, it returns InternalServer Error.
2. Get **api/sensor**: Returns the list of sensors present in the db
3. Get **api/sensor/{id}**: Accepts a sensor Id which is a guid and returns the corresponding sensor object if it is present in the db. If the sensor does not exist in the db, the service returns NoContent. 
4. Post  **api/sensor**: Accepts a sensor object as request body and creates it in the database. 
5. Put  **api/sensor/{id}**: Accepts a sensor object along with its id and updates the corresponding sensor with new sensor data. If the sensor does not exist in the db, returns a not found error.
6. Delete  **api/sensor/{id}**: Accepts a sensorId and deletes the corresponding sensor in the database. In case the sensor does not exist in the database, the service returns a not found error.

## Technologies used

* ASP.NET Core Web api
* Entity Framework Core 
* SQLLite

### Patterns used
* Clean architecture: To ensure clean separation between layers. This makes unit testing and code maintanence easier. 

## Project Structure
Within the sensormanagement solution we have 2 projects:
1. SensorManagement
2. SensorManagement.Tests

SensorMangement contains the source code for the application and SensorManagement.Tests contains the unit tests for each layer of the application.

Within SensorManagement we have 4 layers:
1. Api: Responsible for presentation logics like the controllers. Also the entrypoint lives here: Program.cs + appsettings
2. Application: Manages the buisiness layer logic. This layer contains all the external component interfaces and the services that handle real business logics. We have our application DTO's also defined in this layer.
3. Domain: In this layer we have the entities for our db defined. This helps in clean separation between controllers and infrastructure components. 
4. Infrastructure: The external components are placed in this layer. We have our database and our external platform api calls placed in this layer.


## Table Design
Sensor

* Id : sensor id, primary key, Guid
* Name : Name of the sensor
* Location : Location of the sensor
* CreationTime : Creation time of the sensor
* UpperWarning : Upper warning of the sensor
* LowerWarning : Lower warning of the sensor
