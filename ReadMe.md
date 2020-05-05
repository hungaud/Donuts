Notes while doing this.


1.) Post and Put methods for domain controller - 
I thought about not having the TimeDuration and Length there and allow the "Front End" portion to handle it and when it calls the post method,
the Domain object would already have the date there. But the prompt has Period of registration on there as part of the request.

2.) I had Payment on a different controller so the front end, when it calls post or put methods, it should know to call payment controller to update payment
so that way it decouples domain controller calling payment controller.

3.) I had customer hold publickey and or ContactId so customers can register domain as long as they sign up. and by signing up, they can get verified.
Customers when they sign up are subjected to being verified. if they are not verified, that means Creating a domain will fail.
because the ProviderName will be null. Once they're verified, then it wont be null.


4.) For domain model. I only restricted to minimum of 10 characters. but for more security, there should be max len and regex to verify valid one so no bad
input such as "----". 

5.) for payment, i decided to keep a model so that way we can have a record of each payment. this would end up being very big if its a monthly type of recurring.
the Enum, LenghDuration isnt being used but its meant for specific amount so malicious user can't put invalid numbers for overflow.
Recurring means indefinite until user cancels.

6.) I had a verfication provider model as well because providers could have many different format, or different public key for hashing/decryption etc.

7.) my Put and Post method in domain, I wanted to pass multiple objects from body but i couldn't figure out how to test it with postman so I used Routes for now.
also not sure whats the most correct way so will read into that after this assessment.

8.) post and put domain, Ideally I would like to update the domains expiration date on the front end and have it sent back as new object and 
verify before saving new object. But I wanted to show that expiriation was being changed.

9.) I wrote unit test to test some basic functionalities of the controllers with some test data to show the idea of the designs and how the objects
interact with each other.

Thank you!