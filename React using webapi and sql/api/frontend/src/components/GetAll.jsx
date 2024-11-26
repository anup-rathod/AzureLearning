import React, { useEffect } from 'react'
import axios from 'axios'

function GetAll() {

    useEffect( async() => {
        await axios.get("http://localhost:5034/api/TodoApp/GetNotes")
        .then((res) =>  console.log(res.data))

    }, [])
    

  return (
    <div>
        
    </div>
  )
}

export default GetAll