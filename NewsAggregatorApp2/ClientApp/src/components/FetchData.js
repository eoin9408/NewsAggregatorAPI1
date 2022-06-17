import React, { Component, useState, useEffect } from 'react';


export const FetchData = () => {

    const [newsItems, setNewsItems] = useState([]);
    const [loading, setLoading] = useState(true);


    useEffect(() => {
        const populateNewsData = async () => {
            const rssList = await fetch('https://localhost:7177/api/RssFeeds');
            const rssData = await rssList.json();

            for (const rssfeed of rssData) {
                var apiRequestURL = 'https://localhost:7177/api/NewsItems/rss/' + rssfeed.id;
                const response = await fetch(apiRequestURL, { method: "GET" });
                const data = await response.json();
                const newsItemArray = [...data, ...newsItems].sort((a, b) => new Date(b.articleDateTime).getTime() - new Date(a.articleDateTime).getTime());
                setNewsItems(newsItemArray);
                setLoading(false);
            }
        }

        populateNewsData();
    }, [])


   const  renderNewsTable = () => {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Publisher</th>
                        <th>Headline</th>
                        <th>Summary</th>
                        <th>Date</th>
                    </tr>
                </thead>
                <tbody>
                    {newsItems.map((newsitem, i)  =>
                        <tr key={newsitem.id}>
                            <td>{newsitem.articlePublisher}</td>
                            <td>{newsitem.articleTitle}</td>
                            <td>{newsitem.articleSummary}</td>
                            <td>{newsitem.articleDateTime}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }


    let contents = loading
        ? <p><em>Loading...</em></p>
        : renderNewsTable();

    return (
        <div>
            <h1 id="tableLabel" >News RSS Feed</h1>
            <p>This component demonstrates fetching RSS feed data through an API.</p>
            {contents}
        </div>
    );
}
