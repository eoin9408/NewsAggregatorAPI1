import React, { useState } from 'react';
import "../AddFeed.css";
import { FetchData } from './FetchData';
import { Home } from './Home';

export const AddFeed = (props) => {

    const [rssID, setRssID] = useState("");
    const [rssName, setRssName] = useState("");
    const [rssURL, setRssURL] = useState("");

    const submitRssFeed = async () => {
        const requestBody = { id: rssID, feedName: rssName, feedURL: rssURL }

        const json = JSON.stringify(requestBody);

        const response = await fetch('https://localhost:7177/api/RssFeeds', {
            method: "POST", 
            body: json, 
            headers: {
                "Content-Type":"application/json"
            }
        });

        if(response.status >= 200 && response.status < 300){
            await fetch('https://localhost:7177/api/NewsItems/rss/' + rssID, {method: "POST"});
            setRssID("");
            setRssName("");
            setRssURL("");
        }
    }


    return (
        <div>
            <form class="feed-form" onSubmit={e => submitRssFeed()}>
                <div class="input-grp">
                    <label>RSS ID:</label>
                    <input type="text" value={rssID} onChange={(e) => setRssID(e.currentTarget.value)}/>
                </div>
                <div class="input-grp">
                    <label>RSS Name:</label>
                    <input type="text" value={rssName} onChange={(e) => setRssName(e.currentTarget.value)}/>
                </div>
                <div class="input-grp">
                    <label>RSS URL:</label>
                    <input type="text" value={rssURL} onChange={(e) => setRssURL(e.currentTarget.value)}/>
                </div>
                <div class="btn-grp">
                    <input class="btn" type="submit" value="Submit"/>
                    <button class="btn" onClick={e => props.onFormCancel(e)}>Cancel</button>
                </div>   
            </form>
        </div>
    )
}