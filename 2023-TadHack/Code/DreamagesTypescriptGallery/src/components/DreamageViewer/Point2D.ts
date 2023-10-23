export default class Point2D {

    public xPos:number;
    public yPos:number;

    constructor(x:number, y:number) {
        /*if (x != Math.floor(x) || y != Math.floor(y)) {
            throw new Error("Point values must be integers");
        }*/
        this.xPos = x;
        this.yPos = y;
    }
    
};