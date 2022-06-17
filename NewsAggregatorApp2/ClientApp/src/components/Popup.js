import React, { useState, useEffect } from "react"
import { AddFeed } from "./AddFeed"
import "../Popup.css"

export const Popup = (props) => {

    return (
        <div className="popup-container">    
            <div className="popup">
                {props.children}
            </div>       
        </div>
    )
}