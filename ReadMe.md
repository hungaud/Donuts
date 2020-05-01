Notes while doing this.

1.) Post and Put methods for domain controller - 
I thought about not having the TimeDuration and Length there and allow the "Front End" portion to handle it and when it calls the post method,
the Domain object would already have the date there. But the prompt has Period of registration on there as part of the request.