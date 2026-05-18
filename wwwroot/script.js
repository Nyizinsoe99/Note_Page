const BASE_URL = "http://localhost:5202/Node/Notes";

// --- THEME LOGIC ---
function toggleDarkMode() {
    const isDark = document.documentElement.classList.toggle('dark');
    const icon = document.getElementById('themeIcon');
    if (icon) {
        icon.innerText = isDark ? "☀️ Light Mode" : "🌙 Dark Mode";
    } else {
        document.getElementById('themeBtn').innerText = isDark ? "☀️ Light Mode" : "🌙 Dark Mode";
    }
    localStorage.setItem('theme', isDark ? 'dark' : 'light');
}

if (localStorage.getItem('theme') === 'dark') {
    document.documentElement.classList.add('dark');
    window.addEventListener('DOMContentLoaded', () => {
        const icon = document.getElementById('themeIcon') || document.getElementById('themeBtn');
        if (icon) icon.innerText = "☀️ Light Mode";
    });
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

// *** မင်းဖြစ်ချင်တဲ့ ချက်အကုန်လုံးကို ဤနေရာတွင် စနစ်တကျ ပြင်ဆင်ပေးထားပါသည် ***
async function loadNotes() {
    const response = await fetch(`${BASE_URL}?$orderby=Id desc`);
    const data = await response.json();
    
    const col0 = document.getElementById('col-0');
    const col1 = document.getElementById('col-1');
    const col2 = document.getElementById('col-2');
    
    if(col0) col0.innerHTML = '';
    if(col1) col1.innerHTML = '';
    if(col2) col2.innerHTML = '';

    data.value.forEach((note, index) => {
        const card = document.createElement('div');
        
        // h-auto (စာတိုရင် auto)၊ max-h-[300px] (အမြင့်ဆုံး 300px ပဲ)၊ overflow-hidden (scrollbar လုံးဝဖျောက်ထားသည်)
        card.className = "w-full h-auto max-h-[300px] bg-white dark:bg-slate-900 rounded-2xl border border-slate-200 dark:border-slate-800 shadow-sm overflow-hidden hover:border-blue-400 dark:hover:border-blue-500 cursor-pointer transition-all flex flex-col flex-shrink-0";
        card.onclick = () => showDetails(note);

        // စာသားတွေအောက်ကို ဆင်းသွားရင် scrollbar မပေါ်ဘဲ ကတ်ထဲမှာတင် ငြိမ်နေအောင် ကန့်သတ်ထားပါတယ်
        card.innerHTML = `
            ${note.Photo ? `<div class="w-full max-h-[140px] overflow-hidden flex-shrink-0"><img src="${note.Photo}" class="w-full h-full object-cover"></div>` : ''}
            <div class="p-5 flex-1 flex flex-col overflow-hidden">
                <h3 class="font-bold text-slate-800 dark:text-white mb-2 break-words text-lg leading-snug flex-shrink-0">${note.Title}</h3>
                <p class="text-slate-500 dark:text-slate-400 text-sm whitespace-pre-wrap break-words leading-relaxed flex-1 overflow-hidden">
                    ${note.Content}
                </p>
            </div>
        `;

        // ကော်လံ ၃ ခုထဲကို ဘယ်၊ အလယ်၊ ညာ အလှည့်ကျ မျှထည့်ပေးခြင်း (ဒါမှ ထိပ်ချင်း လုံးဝ ညီမှာပါ)
        const colIndex = index % 3;
        if (colIndex === 0 && col0) col0.appendChild(card);
        else if (colIndex === 1 && col1) col1.appendChild(card);
        else if (colIndex === 2 && col2) col2.appendChild(card);
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
    
    // Modal ကို Screen ရဲ့ အလယ်တည့်တည့်မှာ ပေါ်လာစေရန် flex လို့ ညွှန်ကြားချက်ပေးခြင်း
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