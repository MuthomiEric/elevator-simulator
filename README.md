# Elevator

### Built With

* [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
* [DOCKER](https://www.docker.com/products/docker-desktop)
* [KAFKA](https://kafka.apache.org/documentation/)

# Installing .NET 6

- [Downloads](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Installation docs](https://docs.microsoft.com/dotnet/core/install/)
- [Install Visual studio](https://docs.microsoft.com/en-us/visualstudio/install/install-visual-studio?view=vs-2022)

# Installing DOCKER

- [Downloads](https://www.docker.com/products/docker-desktop/)
- [Installation docs](https://docs.docker.com/?_gl=1*nyfs7v*_ga*MjA3NTE4ODM2NC4xNjg4MTQ5MTg0*_ga_XJWPQMJYHQ*MTY4ODI1MzAwMS4yLjEuMTY4ODI1MzA1NS42LjAuMA..)

# Installing KAFKA on Docker
- Create a docker compose file and paste the code below
- One pulls the kafka zookeeper and the other one pulls kafka server image

```sh
version: '2'
services:
  zookeeper:
    image: confluentinc/cp-zookeeper:latest
    environment:
      ZOOKEEPER_CLIENT_PORT: 2181
      ZOOKEEPER_TICK_TIME: 2000
    ports:
      - 22181:2181
  
  kafka:
    image: confluentinc/cp-kafka:latest
    depends_on:
      - zookeeper
    ports:
      - 29092:29092
    environment:
      KAFKA_BROKER_ID: 1
      KAFKA_ZOOKEEPER_CONNECT: zookeeper:2181
      KAFKA_ADVERTISED_LISTENERS: PLAINTEXT://kafka:9092,PLAINTEXT_HOST://localhost:29092
      KAFKA_LISTENER_SECURITY_PROTOCOL_MAP: PLAINTEXT:PLAINTEXT,PLAINTEXT_HOST:PLAINTEXT
      KAFKA_INTER_BROKER_LISTENER_NAME: PLAINTEXT
      KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR: 1
```
## Getting Started
- The solution has three projects, two are Console apps while the other one is just a class library
- One console is a simulation of user input and the other one simulates the elevator
- You need to set both consoles as startup projects

NB:
At first you might not have kafka topic set so you need to send a message on input console to have one set.

## Contact

Erick Muthomi - erick.muthomi@outlook.com

Project Link: [https://github.com/MuthomiEric/elevator-simulator](https://github.com/MuthomiEric/elevator-simulator)

<p align="right">(<a href="#top">back to top</a>)</p>
