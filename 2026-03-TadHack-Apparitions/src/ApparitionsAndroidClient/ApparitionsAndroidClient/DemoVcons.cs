using ApparitionsAndroidClient.Models;
using Java.IO;
using Newtonsoft.Json;
using Org.Json;

namespace ApparitionsAndroidClient;

public class DemoVcons
{
    public static void InitializeScenario()
    {
        var root = new ScenarioFile();
        
        root.VCons.Add(InitializeGrandfatherVcon());
        root.VCons.Add(InitializeLarryVcon());

        var json = JsonConvert.SerializeObject(root);
        
        System.IO.File.WriteAllText("/home/jurrd3/repos/pockybum522-hackathons/2026-03-TadHack-Apparitions/src/ApparitionsAndroidClient/ApparitionsAndroidClient/Testing/scenario.json", json);
    }
    
    public static VconRoot GaryGrandfatherTreeByGarageVcon => InitializeGrandfatherVcon();
    public static VconRoot LarryVcon => InitializeLarryVcon();

    private static VconRoot InitializeGrandfatherVcon()
    {
        var root = new VconRoot();

        var dialog = new Dialog();

        dialog.Parties = [1];
        
        dialog.Body =  """
                         My grandfather once killed a man in this 
                         garage.
                         
                         It is said that his ghost still haunts it to 
                         this day.
                         
                         Well, that is, my grandfather's ghost. 
                         
                         The ghost of the guy he killed haunts the tree 
                         over there, for some reason.
                         
                         I don't know why I'm telling you this, Larry.
                         
                         Perhaps it's because I want the ghost
                         of the guy he killed to stop haunting my
                         loquat tree.
                         
                         I wonder if talking about my grandfather's 
                         ghost will allow that to happen. I don't 
                         know why that would work, but at this point 
                         I'd do anything. I really want some loquats
                         and as long as his ghost haunts that loquat 
                         tree, there will be no fruit. 
                         
                         I even tried spraying it with olive oil.
                         
                         I don't know why I thought that would help 
                         but now it's just slimy when I prune it.
                         
                         Ha, prunes. That reminds me of my grandfather.
                         
                         Anyways, Larry. You got the $50 you owe me 
                         that you came over to pay me back? You'd 
                         better have it. I think the killing gene is 
                         hereditary and there's no more loquat trees in 
                         this yard. You'd have to haunt the neighbor's 
                         tree, or somethin'...
                         """;
        
        root.Dialog.Add(dialog);

        var locationAttachment = new Attachment();

        locationAttachment.Type = "lat/lon/audio_filename";
            
        // By the shop
        locationAttachment.Body.Add("28.594340");
        locationAttachment.Body.Add("-81.381630");
        locationAttachment.Body.Add("gary_grandfather_garage.mp3");
        
        root.Attachments.Add(locationAttachment);

        return root;
    }
    
    private static VconRoot InitializeLarryVcon()
    {
        var root = new VconRoot();

        var dialog = new Dialog();

        dialog.Parties = [1];
        
        dialog.Body =  """
                         Man it was crazy, I borrowed fifty bucks from 
                         this guy, Gary, and I went over to apologize to
                         him that I needed one more week to pay it back.
                         
                         He was pruning his trees and started talking 
                         to me by a garage. I didn't know if that was 
                         going to be plot relevant but as it turns out,
                         it was.
                         
                         He started telling me about his serial killer
                         grandfather and something about a tree but
                         my eyes were glued to the knife he was pruning 
                         the trees with. Who prunes a tree with a knife?
                         
                         Anyways, I tell him I have to pay him back next
                         week and he starts getting all murdery on me!
                         
                         Chased me down this street and everything, I 
                         can't believe nobody didn't see but the police
                         said they can't do anything.
                         
                         I'm gonna throw the $50 on his front 
                         door step and run once I have it. I ain't
                         going near that guy again if I can help it.
                         
                         Unrelated, I keep hearing weird noises outside
                         my house at night. I think it's probably 
                         squirrels.
                         """;
        
        root.Dialog.Add(dialog);

        var locationAttachment = new Attachment();

        locationAttachment.Type = "lat/lon/audio_filename";
            
        // End of driveway, by street
        locationAttachment.Body.Add("28.594330");
        locationAttachment.Body.Add("-81.382050");
        locationAttachment.Body.Add("larry_about_to_get_murdered.mp3");
        
        root.Attachments.Add(locationAttachment);

        return root;
    }
}