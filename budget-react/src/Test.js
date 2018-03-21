import React from 'react';

export default class Test extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      error: null,
      isLoaded: false,
      items: []
    };
  }

  componentDidMount() {

    var component = this;

    fetch("http://localhost:9686/api/values")
    .then( (response) => {
      return response.json()    
    })
    .then((json) => {
        component.setState({
          isLoaded: true,
          data: json
        })
    })
    .catch(function(ex) {
      component.setState({
        error: ex,
        isLoaded: true
      })
      console.log('parsing failed', ex)
    })
  }

  render() {
    if(this.state.error) {
      return this.stateerror;
    } else if(!this.state.isLoaded) {
      return <div>Loading...</div>
    } else {
      return (        
        <ul>
          {this.state.data.map(i => (
            <li key={i}>
              {i}
            </li>
          ))}
        </ul>
      );
    }
  }
}