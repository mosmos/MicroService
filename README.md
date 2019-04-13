# Microservice API

Serve Application Test Data to Business Users, the data will be used for further analysis by users in there Excel Analysis Reports.

The Business has defined the Percentile Function will use the same algorithm as MS Excel "PERCENTILE.INC" the database is serving data using float(8) and the application has defined the double data type for precision.

The Requirements for this project can be viewed at the following.

* [Business Requirments](/docfx/articles/requirements.md)
* [C# Coding Standards](/docfx/articles/csharp_coding_standards.md)

### SonarQube Code Quaility

[![SonarCloud](http://sonar.navigatorglass.com:9000/api/project_badges/measure?project=db762c49b56bd854f8e7fb1d03f7106468a27387&metric=reliability_rating)](http://sonar.navigatorglass.com:9000/dashboard?id=db762c49b56bd854f8e7fb1d03f7106468a27387)
[![SonarCloud](http://sonar.navigatorglass.com:9000/api/project_badges/measure?project=db762c49b56bd854f8e7fb1d03f7106468a27387&metric=security_rating)](http://sonar.navigatorglass.com:9000/dashboard?id=db762c49b56bd854f8e7fb1d03f7106468a27387)
[![SonarCloud](http://sonar.navigatorglass.com:9000/api/project_badges/measure?project=db762c49b56bd854f8e7fb1d03f7106468a27387&metric=sqale_rating)](http://sonar.navigatorglass.com:9000/dashboard?id=db762c49b56bd854f8e7fb1d03f7106468a27387)

### Docker Hub Images 

 Image       |  Docker Hub | Image Size
------------ | ------------- | -------------
microservice-database | [![This image on DockerHub](https://img.shields.io/docker/pulls/stuartshay/microservice-database.svg)](https://hub.docker.com/r/stuartshay/microservice-database/) |[![](https://images.microbadger.com/badges/image/stuartshay/microservice-database.svg)](https://microbadger.com/images/stuartshay/microservice-database "Get your own image badge on microbadger.com") 
microservice-api-base | [![This image on DockerHub](https://img.shields.io/docker/pulls/stuartshay/microservice-api.svg)](https://hub.docker.com/r/stuartshay/microservice-api/)  | [![](https://images.microbadger.com/badges/image/stuartshay/microservice-api.svg)](https://microbadger.com/images/stuartshay/microservice-api "Get your own image badge on microbadger.com") 
microservice-api-build | [![This image on DockerHub](https://img.shields.io/docker/pulls/stuartshay/microservice-api.svg)](https://hub.docker.com/r/stuartshay/microservice-api/) | [![](https://images.microbadger.com/badges/image/stuartshay/microservice-api.svg)](https://microbadger.com/images/stuartshay/microservice-api "Get your own image badge on microbadger.com")


### MyGet/NuGet Packages

 Package | Status  
------------ | -------------
MicroService.Data | [![MyGet](https://img.shields.io/myget/microservice/v/MicroService.Data.svg)](https://www.myget.org/feed/microservice/package/nuget/MicroService.Data)
MicroService.Service | [![MyGet](https://img.shields.io/myget/microservice/v/MicroService.Service.svg)](https://www.myget.org/feed/microservice/package/nuget/MicroService.Service)

### Jenkins Build Status

 Jenkins | Status  
------------ | -------------
Base Image | [![Build Status](https://jenkins.navigatorglass.com/buildStatus/icon?job=MicroService/microservice-api-base)](https://jenkins.navigatorglass.com/job/MicroService/job/microservice-api-base/)
API  Image | [![Build Status](https://jenkins.navigatorglass.com/buildStatus/icon?job=MicroService/microservice-api-build)](https://jenkins.navigatorglass.com/job/MicroService/job/microservice-api-build/)

##

### Myget Package Deployment

Windows

```powershell
  $env:mygetApiKey = "adab4634-8ddb-4789-ae92-6461295ac69c"
  .\build.ps1 -target push-myget
```

Linux
 
```bash
 export mygetApiKey="adab4634-8ddb-4789-ae92-6461295ac69c"
./build.sh --target=push-myget
```

### DocFX

DocFX generates Documentation directly from source code (.NET, RESTful API, JavaScript, Java, etc...) and Markdown files.

![](assets/docfx.png)


#### Prerequisites:

```
choco install docfx
```

#### Build and Serve Website

```
docfx docfx/docfx.json
docfx docfx/docfx.json --serve
```

```
http://localhost:8080
```

#### Deployment 
```powershell
 .\build.ps1 -target Generate-Docs
```

### SonarQube Testing

Windows

```
 .\build.ps1 -target sonar
```

Linux
```
./build.sh --target sonar
```

### Development Setup

```
stuartshay/microservice-database:v1

docker run —e PGDATA=postgres -p 5432:5432 -i stuartshay/microservice-database:v1

```
