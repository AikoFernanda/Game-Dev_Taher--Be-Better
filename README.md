1. Sinkronisasi Versi Unity & Git LFS<br>
   Supaya tidak terjadi konflik data:
    - Versi Unity: Pastikan menggunakan versi Unity yang sama persis, menggunakan Unity 6.3 LTS (6000.3.5f2).
    - Git LFS (Large File Storage): aset 3D dan modular lmummayan berat, WAJIB sudah menginstal Git LFS di komputer sebelum clone. Jika tidak, aset 3D-nya bisa rusak atau tidak muncul.

3. Pengaturan Project Settings<br>
   Masalah Input System, cek hal ini setelah membuka proyek:
    - Buka Edit > Project Settings > Player.
    - Pastikan Active Input Handling sudah terpasang di Both. Jika masih "New", skrip pergerakan Input.GetAxis tidak akan jalan.

4. Setup Layer & Tags<br>
   Beberapa pengaturan Unity (seperti Layer) terkadang tidak ikut terbawa secara otomatis melalui Git jika belum diatur di dalam folder ProjectSettings.
    - Boy1 (Parent) jadikan layer "Player", dan object lainnya "Default"
    - Pastikan sudah membuat Layer baru bernama "Player". Kalau belum ada, sistem Ground Check di skrip PlayerController bakal bingung.

5. Masalah "Materi Pink" (Render Pipeline)<br>
   Jika ada masalah semua model 3D (termasuk player) berwarna pink menyala.
   - menu Window > Rendering > Render Pipeline Converter.
     
7. .meta Files (Kunci Keharmonisan)<br>
   Pastikan .gitignore sudah benar (tidak mem-block file .meta).
   - Cara ceknya: buka folder proyek di Finder/Explorer, pastikan file .meta ada di samping setiap file aset.
   
9. Scene utama<br>
   - Setelah berhasil membuka project unity, Buka Scene di folder Assets/Scenes/1 untuk mencoba bermain.
   
11. ...
