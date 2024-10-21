// import p5 from 'p5';
// import { createMachine, assign, createActor } from "xstate"
// import axios from "axios";
// import './styles.css';
// import './normalize.css';
// import inkVideo from '../assets/ink_in_water.mp4';
// import energeticVideo from "../assets/energetic_field.mp4";
// import blueEnergyVideo from "../assets/blue_energy.mp4";
// import gentleEnergyFieldVideo from "../assets/gentle_energy_field.mp4";
// import blueEnergeticParticlesVideo from "../assets/blue_energetic_particles.mp4";
// import greenEnergyFieldVideo from "../assets/green_energy_field.mp4";
// import slowedGreenEnergyFieldVideo from "../assets/slowed_green_energy_field.mp4";
// import slowedGreenEnergyFieldFadeoutVideo from "../assets/slowed_green_energy_field_fadeout.mp4";
//
// const sceneTimings = {
//     'SCENE_ONE': 86400,
//     'SCENE_TWO': 15,
//     'SCENE_THREE': 30,
//     'SCENE_FOUR': 30,
//     'SCENE_FIVE': 86400,
//     'SCENE_SIX': 30,
//     'SCENE_SEVEN': 30,
//     'SCENE_EIGHT': 30,
//     'DEFAULT': 86400,
// }
//
// const sceneDefaultConfigs = {
//     'SCENE_ONE': {
//         'videoFLag': false,
//         'x': 10,
//         'y': 15,
//     },
//     'SCENE_TWO': {
//         'x': 10,
//         'y': 15,
//     },
//     'SCENE_THREE': {
//         'x': 10,
//         'y': 15,
//     },
//     'SCENE_FOUR': {
//         'x': 10,
//         'y': 15,
//     },
//     'SCENE_FIVE': {
//         'x': 10,
//         'y': 15,
//     },
//     'SCENE_SIX': {
//         'x': 10,
//         'y': 15,
//     },
//     'SCENE_SEVEN': {
//         'x': 10,
//         'y': 15,
//     },
//     'SCENE_EIGHT': {
//         'x': 10,
//         'y': 15,
//     }
// }
//
// let sceneConfigs = {
//     'SCENE_ONE': {
//         'videoFlag': false,
//         'x': 0,
//         'y': 0,
//     },
//     'SCENE_TWO': {
//         'x': 0,
//         'y': 0,
//     },
//     'SCENE_THREE': {
//         'x': 0,
//         'y': 0,
//     },
//     'SCENE_FOUR': {
//         'x': 0,
//         'y': 0,
//     },
//     'SCENE_FIVE': {
//         'x': 0,
//         'y': 0,
//     },
//     'SCENE_SIX': {
//         'x': 0,
//         'y': 0,
//     },
//     'SCENE_SEVEN': {
//         'x': 0,
//         'y': 0,
//     },
//     'SCENE_EIGHT': {
//         'x': 0,
//         'y': 0,
//     }
// }
//
//
// window.onload = function() {
//     console.log("Window Loaded");
// }
//
// const sceneMachine = createMachine(
//     {
//         context: {
//             sceneNumber: 0,
//         },
//         on: {
//             ADVANCE: {
//                 actions: "assignToScene",
//             },
//             BACKTRACK: {
//                 actions: "assignToScene",
//             },
//             REWIND: {
//                 actions: "assignToScene",
//             }
//         },
//     },
//     {
//     actions: {
//         assignToScene: assign({
//                 sceneNumber: ({ context, event }) => context.sceneNumber + event.advanceBy,
//         })
//     }
// });
//
// const sceneActor = createActor(sceneMachine).start();
//
// sceneActor.subscribe((state) => {
//
//     let sceneTiming = 0;
//
//     switch(state.context.sceneNumber) {
//         case 1:
//             sceneTiming = sceneTimings.SCENE_ONE;
//             break;
//         case 2:
//             sceneTiming = sceneTimings.SCENE_TWO;
//             break;
//         case 3:
//             sceneTiming = sceneTimings.SCENE_THREE;
//             break;
//         case 4:
//             sceneTiming = sceneTimings.SCENE_FOUR;
//             break;
//         case 5:
//             sceneTiming = sceneTimings.SCENE_FIVE;
//             break;
//         case 6:
//             sceneTiming = sceneTimings.SCENE_SIX;
//             break;
//         case 7:
//             sceneTiming = sceneTimings.SCENE_SEVEN;
//             break;
//         case 8:
//             sceneTiming = sceneTimings.SCENE_EIGHT;
//             break;
//         default:
//             sceneTiming = sceneTimings.DEFAULT;
//             break;
//     }
//
//     console.log(`Current scene number: ${state.context.sceneNumber}`);
//
//     if (sceneTimers[state.context.sceneNumber] != null) {
//         sceneTimers[state.context.sceneNumber].clearTimeout();
//         sceneTimers[state.context.sceneNumber].setTimeout(() => {
//             sceneActor.send( {
//                 type: 'ADVANCE',
//                 advanceBy: 1
//             });
//         }, sceneTiming * 1000);
//     } else {
//         // if the previous scene timer is still running, kill the timer
//         if (sceneTimers[state.context.sceneNumber - 1] != null) {
//             sceneTimers[state.context.sceneNumber - 1].clearTimeout();
//         }
//         sceneTimers.push(setTimeout(() => {
//             sceneActor.send( {
//                 type: 'ADVANCE',
//                 advanceBy: 1
//             });
//         }, sceneTiming * 1000));
//     }
// });
//
// let theta = Math.PI;
// let r = 50;
// let x_pos = 100;
// let y_pos = 100;
// let offset = 0;
//
// // For Ed Shear-en
// let i = 0;
//
// let countDownTime = 30;
//
// const maxScenes = 8;
//
// let checkServerTimeout;
// const serverCheckIntervalTime = 250;
//
// let lastEventsList = new Set();
// let newEventsList = new Set();
// let lastTimestampsList = new Set();
// let newTimestampsList = new Set();
//
//
// let sceneTimers = [];
//
// function setSceneConfigsToDefault() {
//     sceneConfigs = structuredClone(sceneDefaultConfigs);
//     //console.log(sceneConfigs);
// }
//
// function findVconsInEvents(events) {
//     //console.log(events);
//     let vCons = [];
//
//     events.forEach(event => {
//         if (event.Type === "VconWithSentiment") {
//             // console.log("vCon detec-a-ted");
//             vCons.push(event);
//         }
//     });
//
//     return vCons;
// }
//
// function findWordsInEvents(events) {
//     let words = [];
//
//     events.forEach(event => {
//         if (event.Type === "NewWordsSpoken") {
//             words.push(event.Data);
//         }
//     });
//
//     return words;
// }
//
// function findInitializeInEvents(events) {
//     let initializes = [];
//
//     events.forEach(event => {
//         if (event.Type === "InitializeEverything") {
//             initializes.push(event.Data);
//         }
//     });
//
//     return initializes;
// }
//
// new p5((p) => {
//
//     let sceneVideos = [
//         p.createVideo(inkVideo).position(0, 0).pause().hide(),
//         p.createVideo(energeticVideo).position(0, 0).pause().hide(),
//         p.createVideo(blueEnergyVideo).position(0, 0).pause().hide(),
//         p.createVideo(gentleEnergyFieldVideo).position(0, 0).pause().hide(),
//         p.createVideo(blueEnergeticParticlesVideo).position(0, 0).pause().hide(),
//         p.createVideo(greenEnergyFieldVideo).position(0, 0).pause().hide(),
//         p.createVideo(slowedGreenEnergyFieldVideo).position(0, 0).pause().hide(),
//         p.createVideo(slowedGreenEnergyFieldFadeoutVideo).position(0, 0).pause().hide()
//     ];
//
//     const removeAllVideos = () => {
//         for (let video in sceneVideos) {
//             console.log(video);
//             video.remove();
//         }
//     }
//
//     p.keyPressed = () => {
//         if (p.key === ' ') {
//             if (sceneActor.getSnapshot().context.sceneNumber >= maxScenes) {
//                 sceneActor.send( {
//                     type: 'REWIND',
//                     advanceBy: -maxScenes
//                 });
//             }
//             sceneActor.send( {
//                 type: 'ADVANCE',
//                 advanceBy: 1
//             });
//         }
//
//         if (p.keyCode === 37) {
//             sceneActor.send({
//                 type: 'BACKTRACK',
//                 advanceBy: -1
//             });
//         }
//     }
//
//     p.windowResized = () => {
//         p.resizeCanvas(p.windowWidth, p.windowHeight);
//     }
//
//     p.setup = async () => {
//         p.createCanvas(p.windowWidth, p.windowHeight);
//
//         //setSceneConfigsToDefault();
//
//         try {
//             checkServerTimeout = setInterval(async () => {
//                 const response = await axios.get("http://192.168.1.118:5001/", {
//                     headers: {
//                         'Access-Control-Allow-Origin': "*",
//                         'Content-Type': 'application/html;charset=utf-8',
//                     }
//                 });
//
//                 newEventsList = new Set(response.data);
//
//                 // create timestamps set from new events
//                 newEventsList.forEach(event => {
//                     newTimestampsList.add(event.Timestamp);
//                 });
//
//                 // create timestamps set from last events
//                 lastEventsList.forEach(event => {
//                     lastTimestampsList.add(event.Timestamp);
//                 })
//
//                 console.log(lastEventsList);
//                 console.log(newEventsList);
//
//                 console.log(`lastTimestampsList Size: ${lastTimestampsList.size}, newTimestampsList Size: ${newTimestampsList.size}`);
//                 console.log(`lastTimestampsList \\ newTimestampsList ${lastTimestampsList.difference(newTimestampsList).size}`);
//                 if (lastTimestampsList.difference(newTimestampsList).size < newTimestampsList.size) {
//                     console.log("out of sync; update lastEventsList");
//                     lastEventsList = new Set([...newEventsList]);
//                 }
//
//                 let initializeCheck = findInitializeInEvents(newEventsList);
//
//                 if (initializeCheck.length > 0 && sceneActor.getSnapshot().context.sceneNumber === 0) {
//                     sceneActor.send( {
//                         type: 'ADVANCE',
//                         advanceBy: 1
//                     });
//                 }
//
//                 let parishionerEnteredCheck = findInitializeInEvents(newEventsList);
//
//                 if (parishionerEnteredCheck.length > 0 && sceneActor.getSnapshot().context.sceneNumber === 1) {
//                     sceneActor.send( {
//                         type: 'ADVANCE',
//                         advanceBy: 1
//                     });
//                 }
//
//                 }, serverCheckIntervalTime);
//             } catch(e) {
//                 console.error(e);
//             }
//
//         console.log(sceneActor.getSnapshot().context.sceneNumber);
//     };
//
//     p.draw = () => {
//         p.background(0);
//         let sceneNumber = sceneActor.getSnapshot().context.sceneNumber;
//
//         let vCons = findVconsInEvents(newEventsList);
//         let spokenWords = findWordsInEvents(newEventsList);
//
//         console.log(spokenWords);
//
//         p.push();
//             p.textSize(30);
//             p.text(`Scene ${sceneActor.getSnapshot().context.sceneNumber}`, 10, 30);
//         p.pop();
//
//         if (sceneNumber === 1) {
//             p.push();
//                 console.log(`Scene One Video Flag: ${sceneConfigs.SCENE_ONE.videoFlag}`);
//                 sceneVideos[sceneNumber].show();
//                 sceneVideos[sceneNumber].play();
//                 p.textSize(50);
//                 //p.text("This scene will contain a soothing ambience", p.windowWidth/2, p.windowHeight/2);
//             p.pop();
//         }
//
//         if (sceneNumber === 2) {
//             sceneVideos[sceneNumber - 1].hide();
//
//             p.push();
//                 sceneVideos[sceneNumber].show();
//                 sceneVideos[sceneNumber].play();
//                 p.textSize(50);
//                 //p.text("This scene will contain an energized ambience", p.windowWidth/2, p.windowHeight/2);
//             p.pop();
//         }
//
//         if (sceneNumber === 3) {
//             sceneVideos[sceneNumber - 1].hide();
//             p.push();
//                 sceneVideos[sceneNumber].show();
//                 sceneVideos[sceneNumber].play();
//                 p.textSize(50);
//                 //p.text("This scene will fade out over the course of 30 seconds", p.windowWidth/2, p.windowHeight/2);
//             p.pop();
//         }
//
//         if (sceneNumber === 4) {
//             sceneVideos[sceneNumber - 1].hide();
//             p.push();
//                 sceneVideos[sceneNumber].show();
//                 sceneVideos[sceneNumber].play();
//                 p.textSize(50);
//                 p.text("SUP DAVID");
//
//                 //p.text("This scene will contain words that appear indistinctly", p.windowWidth/2, p.windowHeight/2);
//             p.pop();
//         }
//
//         if (sceneNumber === 5) {
//             sceneVideos[sceneNumber - 1].hide();
//             p.push();
//                 sceneVideos[sceneNumber].show();
//                 sceneVideos[sceneNumber].play();
//                 p.textSize(50);
//                 //p.text("This scene will contain a white circle that appears in center of screen", p.windowWidth/2, p.windowHeight/2);
//             p.pop();
//         }
//
//         if (sceneNumber === 6) {
//             sceneVideos[sceneNumber - 1].hide();
//             p.push();
//                 sceneVideos[sceneNumber].show();
//                 sceneVideos[sceneNumber].play();
//                 p.textSize(50);
//                 //p.text("Spoken words appear in random places on screen and begin swirling into circle", p.windowWidth/2, p.windowHeight/2);
//             p.pop();
//         }
//
//         if (sceneNumber === 7) {
//             sceneVideos[sceneNumber - 1].hide();
//             p.push();
//                 sceneVideos[sceneNumber].show();
//                 sceneVideos[sceneNumber].play();
//                 p.textSize(50);
//                 //p.text("Prompt to take coin out into world", p.windowWidth/2, p.windowHeight/2);
//             p.pop();
//         }
//
//         if (sceneNumber === 8) {
//             sceneVideos[sceneNumber - 1].hide();
//             p.push();
//                 sceneVideos[sceneNumber].show();
//                 sceneVideos[sceneNumber].play();
//                 p.textSize(50);
//                 //p.text("Scene fades to nothing", p.windowWidth/2, p.windowHeight/2);
//             p.pop();
//         }
//     };
// });