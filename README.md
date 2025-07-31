# Service Name
Automatic Fund Revoking

# Service Architecture

and image of service architecture which shows how service is interacting with databases, distributed caches, message brokers, other services, ...
```
elastic search for logging
redis service for caching
sql server database
this app has some jobs which call Rayan/pasargad bank apis
```

## Development

This part is specified for developers and what they want to share with other developers should be located here


# DevOps

## Dockerfile
- [dockerfile](./Dockerfile)

## Testing

if there are some tests like integration or regression tests should be specified here

### Code quality

if service needs to have some rules for getting checked by code quality service should be inserted here

### Health

health check endpoint of service should be written in this part
```
https://service_url/hc/ready
https://service_url/hc/live
https://service_url/hc/startup
```

### Monitoring Metrics
Here is for specifying monitoring metrics endpoint
```
https://service_url/metrics
```

### Configuration Files
- [dockerfile](./Dockerfile)
- [appsettings.json](/Nitro.Fund.Backend/Presentation/appsettings.json)
- [appsettings.production.json](/Nitro.Fund.Backend/Presentation/appsettings.production.json)
- [appsettings.development.json](/Nitro.Fund.Backend/Presentation/appsettings.development.json)

### Replicas of the pod
the applicable number of pods should be specified here
`Limitless`