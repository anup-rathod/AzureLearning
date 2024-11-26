import React, { useEffect, useState } from 'react'
import axios from 'axios'


function GetAllBlobList() {

  const [content, setContent] = useState([]);

  useEffect(() => {
        axios.get("https://localhost:7041/File/all")
          .then(container => {
              setContent(container.data);
         
          })
          .catch(error => {
              console.error('Error fetching data:', error);
          });
  }, []);

  return (
    <>
      <h1>Get All Blob List</h1>
      <ul>
        {
          content.map((data, index) => (
            <li key={index}>{data}</li>
          ))
        }
      </ul>
    </>
  )
}

export default GetAllBlobList;