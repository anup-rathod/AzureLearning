import axios from 'axios'
import React, { useEffect, useState } from 'react'

function Card() {
    const [content, setContent] = useState([])
    const baseUrl = "https://localhost:7041/File/get?name=";

    useEffect(() => {
        const baseUrl = axios.get("https://localhost:7041/File/all")
        .then((res) => {
        setContent(res.data) 
        })
        .catch((err) => console.log(err))
    }, [])
    


    return(
        <>
        <h2>Card</h2>
        <div style={{display:'flex', justifyContent: 'center', alignItems:'center', }}>
           {  
            content.map((item, index) => (
                    <div style={{marginLeft: '70px'}}>
                    <img src={baseUrl+item} height={150} width={200}/>
                        <li key={index} style={{backgroundColor:'grey', color:'white', border:'2px solid grey' , borderRadius:'20%'}}>{item}</li>
                    </div>
            ))
           }
        </div>
        </>
    )
}

export default Card