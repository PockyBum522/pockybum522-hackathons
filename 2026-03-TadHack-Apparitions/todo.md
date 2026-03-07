- APPARITIONS !?

- MVP - Go to certain place and hear a voice as you get closer to the source. Multiple sources will tell a narrative. Multiple sources can be bundled into scenarios that can be filtered to particular areas. Voice synthesis can be used to create different characters.

- Tech Stack: Avalonia + C#


MVP:

    - Get app running on our phones and building on both computers to be able to test

    - Shows GPS coords real time

    - Can play a sound upon trigger

    - Can change volume of the sound upon trigger

    - MCP and vCons to synthesize voices

    - Specific filterable scenarios per location

    - Text scrolling of what is being said 

    - Grab our VCON C# model and have that handy to serialize/deserialize easily later

  
EP:

    - Camera overlay that can apply effects (to match atmosphere)

    - Take vCons in to build the stories

    - Import vCons that have story built up in the vCons

    - UI Frontend that builds stories that load up the vCons


Dev environment setup:

    - Install Rider
    
    - Will need to run workload restore command per what rider tells you to under "problems"
    
    - Install sdk and android platform 23 through android studio then
    
    - Set android SDK path in Rider > Settings > Build, execution, and deployment > Android to be /home/david/Android/Sdk
    
    - https://download.oracle.com/java/25/archive/jdk-25.0.1_linux-x64_bin.deb
    
    - sudo dpkg -i jdk-25.0.1_linux-x64_bin.deb

    - Set JDK location in Build, execution and deployment to /usr/lib/jvm/jdk-25.0.1-oracle-x64/

    - Go into MainActivity.cs and the issues under "Problems" should go away
    










