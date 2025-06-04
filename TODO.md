[ ] Allow for a calendar in booking flights and set duration for the package price


Notes: I am using Redis.

Right now, when someone books a tour it automatically adds it to the Itinerary (database), and upon clicking "My Itinerary" it shows them what they booked. 

What I need to do is have 'Book Now' on the tours page, it should add it to a cart, when they click on the 'cart' it should have them fill out their information, send a payment to Stripe, and return a confirmation.
At this time, it should be added to 'My Itineraries'