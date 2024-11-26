import React, { useState, useEffect } from 'react'

function Dropdown() {
    const baseUrl = "https://localhost:7041/File/get?name="

    const [img,setImg] = useState(baseUrl)
    const [value, setValue] = useState("");

    useEffect(() => {
        setImg(baseUrl + value);
        console.log(img);
    }, [value]);

    const handleChange = (e) => {
      setValue(e.target.value);
    };

   
  return (
    <div>
        <label htmlFor="imgs" >Choose an img</label>
        <select name="imgs" id="imgs" value={value}  onChange={handleChange}>
        <option value="001.png">Maahi Dan Power</option>
        <option value="005.png">005</option>
        <option value="007.png">007</option>
        </select>
        <p>{`You selected ${value}`}</p>
        <img src={img} alt="" height="100px"/>
    </div>
  )
}

export default Dropdown