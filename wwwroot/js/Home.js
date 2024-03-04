const createNewBtn = document.getElementById('createNewBtn');
const uploadModal = document.getElementById('uploadModal');
const closeModal = document.getElementById('closeModal');
const pdfInput = document.getElementById('pdfInput');
const uploadBtn = document.getElementById('uploadBtn');

createNewBtn.addEventListener('click', () => {
    uploadModal.style.display = 'block';
});

closeModal.addEventListener('click', () => {
    uploadModal.style.display = 'none';
});

window.addEventListener('click', (event) => {
    if (event.target === uploadModal) {
        uploadModal.style.display = 'none';
    }
});

uploadBtn.addEventListener('click', () => {
    const file = pdfInput.files[0];
    if (file) {
        // Handle file upload logic here (e.g., send file to server)
        console.log('File uploaded:', file.name);
        uploadModal.style.display = 'none';
    } else {
        alert('Please select a PDF file to upload.');
    }
});
