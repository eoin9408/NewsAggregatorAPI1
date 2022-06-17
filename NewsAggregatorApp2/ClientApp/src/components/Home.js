import React, { Component, useState, useEffect } from 'react';
import { AddFeed } from './AddFeed';
import { Popup } from './Popup';

export const Home = () => {

    const [popupState, setPopupState] = useState(false);
    const [loading, setLoading] = useState(true);
    const [rssFeeds, setRssFeeds] = useState([]);



    useEffect(() => {

        const populateRSSData = async () => {
            const rssList = await fetch('https://localhost:7177/api/RssFeeds');
            const rssData = await rssList.json();
            setRssFeeds(rssData);
            setLoading(false);
        }

        populateRSSData();
    }, [])


    const renderRSSTable = () => {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Publisher</th>
                        <th>URL</th>
                    </tr>
                </thead>
                <tbody>
                    {rssFeeds.map(rssfeed =>
                        <tr key={rssfeed.ID}>
                            <td>{rssfeed.feedName}</td>
                            <td>{rssfeed.feedURL}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    const handleButtonClick = () => {
        setPopupState(!popupState);
    }


    let contents = loading
        ? <p><em>Loading...</em></p>
        : renderRSSTable();



    return (
        <div>
            <h1>Current RSS Feeds</h1>
            <p>RSS feeds currently added to the database:</p>
            <button className="addFeedButton" onClick={(e) => handleButtonClick(e)}>Add RSS Feed</button>
            {popupState ? (
                <Popup>
                    <AddFeed onFormCancel={
                        e => setPopupState(false)
                    }/>
                </Popup>
            ) : (
                <div></div>
            )}
            {contents}

            <ul>
                <li><a href='https://get.asp.net/'>ASP.NET Core</a> and <a href='https://msdn.microsoft.com/en-us/library/67ef8sbd.aspx'>C#</a> for cross-platform server-side code</li>
                <li><a href='https://facebook.github.io/react/'>React</a> for client-side code</li>
                <li><a href='http://getbootstrap.com/'>Bootstrap</a> for layout and styling</li>
            </ul>
        </div>
    );
}
