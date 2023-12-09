Interactive Rock : SETUP GUIDE
Dear Unity Developer,
Thank you for downloading this package and supporting my work!

This Asset contains:

- 3 rock loops with different intensity levels.

- An automated script to transition between the intensity levels and trigger the loops.

- A "demo" scene to test the script.

How to use the script inside the "Scripts/rock_free" folder:
-------------------------------------------------

The "rock_free" script are inside the "Scripts/rock_free" folder.
All you need to do is to drag and drop the script on a game object with an audio listener component.  
Drag/drop the "rock_mixer" mixer in the same folder into the corresponding box in the script.

Upon attaching the script to a game object, you will see several checkboxes which are public booleans that activate or deactivate the playback of the different samples.

So in your game, whenever you want to activate one of the sample loops (linked to a specific event, trigger...) 
simply access the booleans
 listed inside the script and set one of them to "true".  

As an example, in your game, you want to start the music at a soft intensity, you set the "Soft" boolean to "true".
If there is some action inside your game, set the "Forte" boolean to true and set the "Soft" and "Med" booleans to "false".
Now, there is a moment of pause in your game, make sure all booleans are "false" and the music will fade out.

The intensity level boolean names are, in order, "soft", "med", "forte". Keep it in mind when calling the booleans inside a script.
The changes will only happen at "natural" transition points within the music so plan changes in the mood a little ahead in your game!


Example of code that you need to include inside the script that has your triggers/events:
-----------------------------------------------------------------------------------------



//creates a public variable - drag and drop the game object to which the "Battle_master" script 
is attached into the cell created in your script.


public Rock_free rock_free_script;
//drop the GameObject which has the "rock_free" script attached to it in this box in the inspector.

public bool no_enemies;


void Update(){
	
	if (no_enemies) {
		rock_free.soft = true;
		rock_free.med = false;
		rock_free.forte = false;
		
	}

}
// sets the booleans inside the "rock_free" script to play the "soft" samples if there are no enemies present.



If you have any trouble using the samples, let me know and I will be happy to help!

********************************************************************************************************************************************************************************************************
GENERAL REMARKS:

- DO NOT, under any circumstance, change the folder structure of the "Resources" folder.  The resources folder HAS to be located in your asset folder and the folder structure HAS to remain untouched.
  The reason is that many scrips rely on this folder structure to load the samples into arrays. Many scripts could stop working if you change the folder structure.

- Be sure to add an audio listener to your camera or the game object you attach the script on.  Apparently, this helps with the overall performance of the script.

*********************************************************************************************************************************************************************************************************

-------------------------------------------------------------------------------------------------------------------------
IMPORTANT THINGS TO REMEMBER:

- Make sure only ONE boolean is set to "true" at all times, otherwise you'll hear a mishmash of two or more tracks!
- Whenever you set a boolean to "false", the script waits until the musical phrase is over. This can take some time so be patient! :-)

-------------------------------------------------------------------------------------------------------------------------

I hope you'll be able to make use of this!
Don't forget to leave a review and suggestions/ideas for how to make this work even better!

Thanks again for your support and don't hesitate to contact me if you have any questions/suggestions! 

sincerely,

Marma

CONTACT: marma.developer@gmail.com
WEBSITE: http://marmadeveloper.wix.com/marmamusic