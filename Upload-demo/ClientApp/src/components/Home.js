import React, { Component } from 'react';
import axios  from 'axios';

export class Home extends Component {
  static displayName = Home.name;

  constructor(props) {
    super(props);
    this.state ={
      file: null
    }

    this.onFormSubmit = this.onFormSubmit.bind(this)
    this.onChange = this.onChange.bind(this)
    this.fileUpload = this.fileUpload.bind(this)
  }

  onFormSubmit(e){
    debugger;
    e.preventDefault() 
    this.fileUpload(this.state.file).then((response)=>{
      console.log(response.data);
    })
  }
  onChange(e) {
    this.setState({file:e.target.files[0]})
  }

  fileUpload(file){
    const url = 'weatherforecast';

    const formData = new FormData();
    formData.append('Id',1);
    formData.append('Fichero',file);

    const config = {
        headers: {
            'content-type': 'multipart/form-data'
        }
    }
    return  axios.post(url, formData,config)
  }

  render () {
    return (
      <div>
      <form onSubmit={this.onFormSubmit}>
        <h1>File Upload</h1>
        <input type="file" onChange={(e) => this.onChange(e)} />
        <button type="submit">Upload</button>
      </form>
      </div>
    );
  }
}
