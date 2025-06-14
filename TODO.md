[ ] Allow for a calendar in booking flights and set duration for the package price
[ ] When a user books 3 seats but hasn't checked out, maybe a countdown timer for them to hurry and book? Or something to indicate that 3 seats are no longer available in case other users try to book at the same time.



FlyToSpace/
├── API/
│   └── Dockerfile
├── API.Application/
├── API.Domain/
├── API.Infrastructure/
├── client/
│   ├── Dockerfile
│   └── nginx.conf
├── docker-compose.yml
├── FlyToSpace.sln



cd /path/to/FlyToSpace
docker-compose build
docker-compose up -d
