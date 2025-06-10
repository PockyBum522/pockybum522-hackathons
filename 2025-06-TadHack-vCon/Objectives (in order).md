1) ~~Take a single-ass picture~~ 
	1) Load a picture in such a way that it can be sent to an LLM
	2) Find an LLM that takes pictures and can be asked questions about the picture that can be called using an API
	3) Test through an API call - uploading a picture and asking questions about it
2) ~~Determine what is the most prominent data
	1) Test API call (same task as above)
3) ~~Return most prominent data as transcribed vCon
	1) Look at example vCons and figure out things that we want to store
		1) Date/time
		2) GPS location
		3) Category of group determined by the LLM
		4) Parties (like, the people) (initially will just be LLM, but later could be expanded to US giving back input about a picture)
		5) Save the actual picture in the vCon as Base64 (for later if you want to go through and categorize it later (not MVP but would absolutely whoop ass))
4) Dump the relevant context and data into a plaintext file for Obsidian to read
	1) Pictures taken within a certain time range (MVP)
	2) If the LLM is good at categorizing we can also format based on the category (not MVP)
	3) Find all related vCons (using something similar to one of the above two options)
	4) Dump all the Main Text from the vCons per the above into the note (done; simplicity itself)
