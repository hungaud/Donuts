Notes while doing this.


1.) I wasn't sure what or how to handle verified contact so I commented out Pseudo Code. I understand what it was supposed to do, but didnt know
if I was supposed to call an external API for verification or how it works.

2.) Post and Put methods for domain controller - 
I thought about not having the TimeDuration and Length there and allow the "Front End" portion to handle it and when it calls the post method,
the Domain object would already have the date there. But the prompt has Period of registration on there as part of the request.

3.) I had Payment on a different controller so the front end, when it calls post or put methods, it should know to call payment controller to update payment
so that way it decouples domain controller calling payment controller.

