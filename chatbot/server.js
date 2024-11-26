const express = require('express');
const bodyParser = require('body-parser');
const { AzureOpenAI } = require('openai');
const cors = require('cors');
const fs = require('fs');
const path = require('path');
const { BlobServiceClient } = require('@azure/storage-blob'); // Uncommented this
const app = express();
const port = process.env.PORT || 5000;

// Middleware
app.use(bodyParser.json());
app.use(cors()); // Use cors middleware to allow all origins

// Azure OpenAI initialization
const apiKey = process.env.AZURE_OPENAI_API_KEY || "1fac5591063f45efab2df26dac051a34";
const endpoint = process.env.AZURE_OPENAI_ENDPOINT || "https://chatbotfordemosales.openai.azure.com/";
const deployment = "chatbotdemoforsales";
const apiVersion = "2024-04-01-preview";

const client = new AzureOpenAI({
  endpoint: endpoint,
  apiKey: apiKey,
  deployment: deployment,
  apiVersion: apiVersion,
});


// Endpoint to handle prompt processing
// app.post('/api/send-prompt', async (req, res) => {
//   const { prompt, fileContent } = req.body;
//   console.log(prompt)
//   const fileData = fileContent.toString('utf-8');
//   // console.log(fileData,"data")
//   // console.log(fileContent)
//   try {
//     const result = await client.chat.completions.create({
//       messages: [
//         { role: "system", content: `You are an AI assistant. Use only the provided content to respond to the user. If the requested information is not in the provided content, respond with: "The file does not contain information about the given prompt." ${fileData}` },
//         { role: "user", content: prompt },
//       ],
//       model: 'chatbotdemoforsales',
//     });

//     const response = result.choices[0].message.content;
//     console.log(response);
//     res.json({ response });
//   } catch (error) {
//     console.error('Error processing prompt:', error.message);
//     res.status(500).json({ error: 'Failed to process prompt' });
//   }
// });

// // Start server
// app.listen(port, () => {
//   console.log(`Server running on port ${port}`);
// });

// Azure Blob Storage configuration
const blobServiceClient = BlobServiceClient.fromConnectionString(
  process.env.AZURE_STORAGE_CONNECTION_STRING || 
  "DefaultEndpointsProtocol=https;AccountName=prodchatbotcontainer;AccountKey=pr9l2DvBWb9CON3f6A4gcmz+SICLxjLS5WusP/yjqOrPzmhBu1TT8wVoy7qL82EvyqqQWipOHRSo+AStiMYPPg==;EndpointSuffix=core.windows.net"
);
const containerName = "containerprod"; 

// Endpoint to handle prompt processing
app.post('/api/send-prompt', async (req, res) => {
  const { prompt } = req.body;

  try {
    // Fetch file from Azure Blob Storage
    const blobName = "filem.txt"; 
    const containerClient = blobServiceClient.getContainerClient(containerName);
    const blobClient = containerClient.getBlockBlobClient(blobName);
    const downloadBlockBlobResponse = await blobClient.downloadToBuffer();

    // Process file content
    const fileContent = downloadBlockBlobResponse.toString('utf-8');

    // Send prompt to Azure OpenAI with file content
    const result = await client.chat.completions.create({
      messages: [
        {
          role: "system", 
          content: `You are an AI assistant. Use only the provided content to respond to the user. If the requested information is not in the provided content, respond with: "The file does not contain information about the given prompt." ${fileContent}`
        },
        {
          role: "user", 
          content: prompt
        }
      ],
      model: 'chatbotdemoforsales',
    });

    const response = result.choices[0].message.content;
    console.log(response);
    res.json({ response });
  } catch (error) {
    console.error('Error processing prompt:', error.message);
    res.status(500).json({ error: 'Failed to process prompt' });
  }
});

// Start server
app.listen(port, () => {
  console.log(`Server running on port ${port}`);
});
