1. Sinkronisasi Versi Unity & Git LFS
    agar tidak terjadi konflik data:
    - Versi Unity: Pastikan menggunakan versi Unity yang sama persis, menggunakan Unity 6.3 LTS (6000.3.5f2).
    - Git LFS (Large File Storage): aset 3D dan modular lmummayan berat, WAJIB sudah menginstal Git LFS di komputer sebelum clone. Jika tidak, aset 3D-nya bisa rusak atau tidak muncul.

2. Pengaturan Project Settings
    masalah Input System, cek hal ini setelah membuka proyek:
    - Buka Edit > Project Settings > Player.
    - Pastikan Active Input Handling sudah terpasang di Both. Jika masih "New", skrip pergerakan Input.GetAxis tidak akan jalan.

3. Setup Layer & Tags
    Beberapa pengaturan Unity (seperti Layer) terkadang tidak ikut terbawa secara otomatis melalui Git jika belum diatur di dalam folder ProjectSettings.
    - Boy1 (Parent) jadikan layer "Player", dan object lainnya "Default"
    - Pastikan sudah membuat Layer baru bernama "Player". Kalau belum ada, sistem Ground Check di skrip PlayerController bakal bingung.
