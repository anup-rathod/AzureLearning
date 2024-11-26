import axios from 'axios';
import React, { useEffect, useState } from 'react'

function UploadImg() {
    const [file, setFile] = useState([])

    
    function handleChange(event){
        setFile(event.target.files[0])
        console.log(event.target.files[0])
    }


    function handleSubmit(e) {
        e.preventDefault();
        console.log("hello")
        const url = 'https://localhost:7041/File/upload';

    
       
        formData.append('file', file)[0].files[0]
        console.log(formData);
       
            axios.post(url, formData).then((res) =>{
                console.log(res)
            })

        console.log(file)

    }
  return (
    <div>
        <h2 style={{backgroundColor:'yellowgreen', }}>File Upload</h2>
        <form >
            <input type="file" name='file' onChange={handleChange}/>
            <button onClick={handleSubmit}>Submit</button>
        </form>
    </div>
  )
}

export default UploadImg