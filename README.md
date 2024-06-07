
# Platform Service Microservice

This project refers to two microservices developed as part of a portfolio to show microservices comprehension.

The Services listed are as below:
* ### **Platform Service**
A service that collects various platforms and frameworks that are used by people in the technical field

* ### **Command Service**
A repository for all the commands of all the different platforms that have been listed. Users can create new commands for specified platforms and get commands for the same

The project above demostrates a level of sufficiency in:
* Creating REST APIs in controllers by following MVC pattern as well as the repository pattern
* Deploying each of the services onto Docker
* Creating the required Kubernetes Resources to deploy the services, database(MSSQL), link them to each other using ClusterIPs as well as develop an API Gateway using Ingress Nginx to access them
* Develop a message bus using RabbitMQ that will link the services using Publisher-Subscriber Pattern. When the user creates a new Platform on Platform Service, it publishes it onto the message bus. The CommandService will receive the new platform as it is subscribed, deserialize it and save it to it's database
## Tech Stack

* **Server:** ASP.NET Core, C#
* **Database:** SQL Server, In-Memory Database
* **Container Creation and Orchestration**: Docker and Kubernetes
* **API Gateway Integration**: Ingress Nginx
* **Inter Service Communication**: RabbitMQ and GRPC

