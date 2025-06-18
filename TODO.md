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


Deployment Issues:
[ ] Registration cancel doesn't redirect
[ ] How to have the CSS on Register notice that the keypad for typing is covering up areas. 
[ ] Why does the address not get recognized by google manager fr this site, but it does on others? it'll ask to save, but it doesn't recognize the address field
[X] Also, when i try to submit a form and it says an account already exists, i try changing the email address and it doesn't recognize the new input
[ ] Navbar, how to make the navbar automatically go up after a few seconds if they click a link
[X] Spaceship doesn't align center in mobile view
[X] New users are reportin the account already exists
[X] Incorporate 'Show Password'
[ ] Its not saying incorrect/invalid login/password
