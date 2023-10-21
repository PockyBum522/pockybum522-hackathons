import React, { useState, useEffect, useRef } from 'react';
//import ReactDOM from 'react-dom/client'
// 2048 x 1536
// 1080/6
// 768/6

// figure out which dimension is longer
// anchor by constraining dimension

const DreamageViewer = () => {

    // states
    const [xPos, setXPos] = useState(0);
    const [yPos, setYPos] = useState(0);
    const [xDir, setXDir] = useState(1);
    const [yDir, setYDir] = useState(1);
    const [currWinWidth, setCurrWinWidth] = useState(window.innerWidth);
    const [currWinHeight, setCurrWinHeight] = useState(window.innerHeight);
    
    // refs
    const mainImgEl = useRef<HTMLImageElement>(null);
    
    useEffect(() => {
        
        const timer = setTimeout(() => {
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
        }, 1)
    }, [xPos, yPos]);
    
    useEffect(() => {
        
    }, [currWinWidth]);
    
    useEffect(() => {
        
    }, [currWinHeight]);
    
    return (
        <div className={"relative"}>
            <img id="main_img" ref={mainImgEl} style={{"top": yPos, "left": xPos, "width": Math.floor(window.innerWidth / 2)}} className={`absolute rounded-sm`} src={`https://placehold.co/2048x1536`} alt={"placeholder"}></img>
        </div>
    )
}

export default DreamageViewer;