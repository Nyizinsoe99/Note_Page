const BASE_URL = "http://localhost:5202/Node/Notes";

// --- THEME LOGIC ---
function toggleDarkMode() {
    const isDark = document.documentElement.classList.toggle('dark');
    const icon = document.getElementById('themeIcon');
    icon.innerText = isDark ? "☀️ Light Mode" : "🌙 Dark Mode";
    localStorage.setItem('theme', isDark ? 'dark' : 'light');
}

if (localStorage.getItem('theme') === 'dark') {
    document.documentElement.classList.add('dark');
}

// --- PHOTO PREVIEW LOGIC ---
document.getElementById('photoInput').addEventListener('change', async function() {
    const file = this.files[0];
    const preview = document.getElementById('imagePreview');
    if (file) {
        const base64 = await toBase64(file);
        preview.src = base64;
        preview.classList.remove('hidden');
    } else {
        preview.classList.add('hidden');
    }
});

const toBase64 = file => new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => resolve(reader.result);
    reader.onerror = error => reject(error);
});

// --- DATA LOGIC ---
async function saveNote() {
    const title = document.getElementById('title').value;
    const content = document.getElementById('content').value;
    const photoFile = document.getElementById('photoInput').files[0];

    if (!title || !content) return alert("Please fill all fields!");

    let photoBase64 = photoFile ? await toBase64(photoFile) : null;

    await fetch(BASE_URL, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ Title: title, Content: content, Photo: photoBase64 })
    });

    document.getElementById('title').value = '';
    document.getElementById('content').value = '';
    document.getElementById('photoInput').value = '';
    document.getElementById('imagePreview').classList.add('hidden');
    loadNotes();
}

async function loadNotes() {
    const response = await fetch(`${BASE_URL}?$orderby=Id desc`);
    const data = await response.json();
    const listDiv = document.getElementById('notesList');
    listDiv.innerHTML = '';

    data.value.forEach(note => {
        const card = document.createElement('div');
        // break-inside-avoid က masonry layout မှာ box တွေ အလယ်ကနေ ပြတ်မသွားအောင် လုပ်ပေးပါတယ်
        // h-fit နဲ့ mb-6 က box ရဲ့ content အလိုက် အမြင့်ကို ချိန်ညှိပေးပါတယ်
        card.className = "break-inside-avoid inline-block w-full mb-6 bg-white dark:bg-slate-900 rounded-2xl border border-slate-200 dark:border-slate-800 shadow-sm overflow-hidden hover:border-blue-400 dark:hover:border-blue-500 cursor-pointer transition-all";
        card.onclick = () => showDetails(note);

        card.innerHTML = `
            ${note.Photo ? `<img src="${note.Photo}" class="w-full h-auto object-cover">` : ''}
            <div class="p-5">
                <h3 class="font-bold text-slate-800 dark:text-white mb-2 break-words text-lg">${note.Title}</h3>
                <p class="text-slate-500 dark:text-slate-400 text-sm whitespace-pre-wrap break-words leading-relaxed">${note.Content}</p>
            </div>
        `;
        listDiv.appendChild(card);
    });
}

function showDetails(note) {
    document.getElementById('modalTitle').innerText = note.Title;
    document.getElementById('modalContent').innerText = note.Content;
    const modalImg = document.getElementById('modalImage');
    
    if (note.Photo) {
        modalImg.src = note.Photo;
        modalImg.classList.remove('hidden');
    } else {
        modalImg.classList.add('hidden');
    }

    document.getElementById('deleteBtn').onclick = () => deleteNote(note.Id);
    const modal = document.getElementById('noteModal');
    modal.classList.remove('hidden');
    modal.classList.add('flex');
}

function closeModal() {
    const modal = document.getElementById('noteModal');
    modal.classList.add('hidden');
    modal.classList.remove('flex');
}

async function deleteNote(id) {
    if (!confirm("Delete?")) return;
    await fetch(`${BASE_URL}(${id})`, { method: 'DELETE' });
    closeModal();
    loadNotes();
}

loadNotes();