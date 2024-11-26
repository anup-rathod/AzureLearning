import axios from 'axios'
import React, { useEffect , useState } from 'react'

function GetImage() {
    const baseUrl = "https://localhost:7041/File/get?name=";

    const [formData, setFormData] = useState({
        inp: "",
      })
    const [imgUrl, setImgUrl] = useState(baseUrl)

    const handleChange = (event) => {
        setFormData( (curr) => {
            return { ...curr, [ event.target.name] : event.target.value}
        })
    }


    

    const handleSubmit = () => {
        setImgUrl(baseUrl+formData.inp) 
     
    }

  return (
    <div>
        <h1>Image</h1>
        <input type="text" name='inp' value={formData.inp} onChange={handleChange}/>
        <img src={imgUrl} alt="" height="100px"/>
        <button onClick={handleSubmit}>Get Image</button>
    </div>
  )
}

export default GetImage