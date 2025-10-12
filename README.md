```bash
git lfs install
git lfs track "*.png" "*.jpg" "*.ogg" "*.mp3" "*.wav"
git add .gitattributes
git add .
git commit -m "Fix .gitattributes for Godot and LFS"
git lfs pull
git lfs checkout
rm -rf .godot/imported
```
