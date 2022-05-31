import React, { Component } from 'react';

export class FetchData extends Component {
  static displayName = FetchData.name;


  constructor(props) {
    super(props);
      this.state = { newsitems: [], loading: true };
  }


  componentDidMount() {
      this.populateNewsData();
  }


  static renderNewsTable(newsitems) {
      return (
          <table className='table table-striped' aria-labelledby="tabelLabel">
              <thead>
                  <tr>
                      <th>ID</th>
                      <th>Headline</th>
                      <th>Summary</th>
                      <th>Date</th>
                  </tr>
              </thead>
              <tbody>
                  {newsitems.map(newsitem =>
                      <tr key={newsitem.id}>
                          <td>{newsitem.id}</td>
                          <td>{newsitem.articleTitle}</td>
                          <td>{newsitem.articleSummary}</td>
                          <td>{newsitem.articleDateTime}</td>
                      </tr>
                  )}
              </tbody>
          </table>      
          );
  }


  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
          : FetchData.renderNewsTable(this.state.newsitems);

    return (
      <div>
        <h1 id="tabelLabel" >News RSS Feed</h1>
        <p>This component demonstrates fetching RSS feed data through an API.</p>
        {contents}
      </div>
    );
  }


  async populateNewsData() {
      const response = await fetch('https://localhost:7177/api/NewsItems/rss');
      const data = await response.json();
      this.setState({ newsitems: data, loading: false });
  }
}
