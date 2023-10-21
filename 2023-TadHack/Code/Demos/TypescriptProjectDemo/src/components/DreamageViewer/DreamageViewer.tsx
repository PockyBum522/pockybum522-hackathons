import React, { useState, useEffect } from 'react';
//import ReactDOM from 'react-dom/client'
// 2048 x 1536
const DreamageViewer = () => {

    const [xPos, setXPos] = useState(10);
    const [yPos, setYPos] = useState(10);
    const [xDir, setXDir] = useState(1);
    const [yDir, setYDir] = useState(-1);
    
    
    useEffect(() => {
        const timer = setTimeout(() => {
            setXPos(xPos+xDir);
            setYPos(yPos+yDir);
        }, 15)
    }, [xPos, yPos]);
    
    return (
        <div className={"relative"}>
            <img style={{"top": yPos, "left": xPos}} className={`absolute rounded-sm`} src={"https://placehold.co/600x400"} alt={"placeholder"}></img>
        </div>
    )
}

export default DreamageViewer;