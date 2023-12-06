import React, { useState, useEffect, useRef } from 'react';
import Point2D from './Point2D';
import axios from 'axios';
// 2048 x 1536
// 1080/6
// 768/6

// figure out which dimension is longer
// anchor by constraining dimension

const DreamageViewer = () => {

    // local consts
    const refreshRateImgMovement = 10; // in milliseconds
    const refreshRateImgCarousel = 2000; // in milliseconds
    
    // states
    const [xPos, setXPos] = useState(0);
    const [yPos, setYPos] = useState(0);
    const [xDir, setXDir] = useState(1);
    const [yDir, setYDir] = useState(1);
    const [screenImageCount, setScreenImageCount] = useState(6);
    const [posArray, setPosArray] = useState<Point2D[]>([]);
    const [currImgIndex, setCurrImgIndex] = useState(0);
    const [currWinWidth, setCurrWinWidth] = useState(window.innerWidth);
    const [currWinHeight, setCurrWinHeight] = useState(window.innerHeight);
    const [imagePathArray, setimagePathArray] = useState<string[]>([]);
    const [imageTagsArray, setImageTagsArray] = useState<React.ReactElement[]>([]); 
    
    // refs
    const mainImgEl = useRef<HTMLImageElement>(null);
    
    useEffect(() => {
        
       axios.get("http://pockybum522.com:8080/images/images_index.json")
           
           .then((response) => {
               
               const filenames = response.data.images_filenames;
               let filenamesArray:string[] = [];
               let randomPosArray:Point2D[] = [];
               
               //console.log(filenames)
               
               for (let el in filenames) {
                   filenamesArray.push(filenames[el]);
                   
               }
               
               // need to filter the array to only allow for the png image files and nothing else
               const filenamesArrayFiltered = filenamesArray.filter((path) => /\.(jpe?g|png|webp)$/.test(path));
               
               // generate random position array for the images
               for (let filename in filenamesArrayFiltered) {
                   if (mainImgEl.current !== null) {
                       const randomXPos = Math.floor(Math.abs(Math.random() * currWinWidth - mainImgEl.current.getBoundingClientRect().right));
                       const randomYPos = Math.floor(Math.abs(Math.random() * currWinHeight - mainImgEl.current.getBoundingClientRect().right));
                       const randomPos = {xPos: randomXPos, yPos: randomYPos};
                       randomPosArray.push(randomPos);
                   }
               }
               
               setimagePathArray(filenamesArrayFiltered);
               setPosArray(randomPosArray);
               
               
               // iterate through random image positions and set the positions of each
               // React img tag; collect them into an array of {screenImageCount} elements
               let stagingImageTagsArray:React.ReactElement[] = [];
               
               for (let i = 0; i < screenImageCount; i++) {
                   if (posArray[i] !== undefined) {
                       console.log(`${posArray[i].xPos}, ${posArray[i].yPos}`);

                       stagingImageTagsArray.push(
                           <img
                               id="main_img"
                               ref={mainImgEl}
                               style={{"top": posArray[i].yPos, "left": posArray[i].xPos, "width": Math.floor(window.innerWidth / 2)}}
                               className={`absolute rounded-lg`}
                               src={`${PROTOCOL}://${DOMAIN}:${PORT}/images/${imagePathArray[currImgIndex]}`}
                               alt={"placeholder"}>
                           </img>
                       );
                   }
               }
               
               setImageTagsArray(stagingImageTagsArray);
               
           });
    }, []);
    
    useEffect(() => {
        
        const carouselTimer = setTimeout(() => {
            
            console.log(`${currImgIndex} > ${imagePathArray.length}`);
            
            if (currImgIndex === imagePathArray.length - 1)
                setCurrImgIndex(0);
            else 
                setCurrImgIndex(currImgIndex + 1);
            
        }, refreshRateImgCarousel)
        
    }, [currImgIndex]);
    
    useEffect(() => {
        
        console.log(imagePathArray);
        
    }, [imagePathArray]);
    
    useEffect(() => {
        
        const imgMovementTimer = setTimeout(() => {
            
            
            if (mainImgEl.current !== null) {
                if (yPos + mainImgEl.current.getBoundingClientRect().height >= currWinHeight)
                    setYDir(-1);
                if (yPos < 0)
                    setYDir(1);
                if (xPos + mainImgEl.current.getBoundingClientRect().width >= currWinWidth)
                    setXDir(-1);
                if (xPos < 0)
                    setXDir(1);
            }
            
            setYPos(yPos + yDir);
            setXPos(xPos + xDir);
            
        }, refreshRateImgMovement)
        
    }, [xPos, yPos]);
    
    useEffect(() => {
        
    }, [currWinWidth]);
    
    useEffect(() => {
        
    }, [currWinHeight]);

    const PROTOCOL = "http";
    //const DOMAIN = "pockybum522.com";
    const DOMAIN = "100.110.42.90";
    const PORT = "8080";
    
    return (
        <div className={"relative"}>
            <img id="main_img" ref={mainImgEl} style={{"top": yPos, "left": xPos, "width": Math.floor(window.innerWidth / 2)}} className={`absolute rounded-lg`} src={`${PROTOCOL}://${DOMAIN}:${PORT}/images/${imagePathArray[currImgIndex]}`} alt={"placeholder"}></img>
            { /*imageTagsArray*/ }
        </div>
    )
}

export default DreamageViewer;