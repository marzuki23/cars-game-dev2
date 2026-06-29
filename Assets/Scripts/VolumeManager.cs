using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeManager : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource musicSource;

    [Header("UI")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TMP_Text volumeText;

    private void Start()
    {
        // Atur slider
        volumeSlider.minValue = 0;
        volumeSlider.maxValue = 100;
        volumeSlider.wholeNumbers = true;

        // Ambil volume yang tersimpan
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 50);

        volumeSlider.value = savedVolume;

        // Atur volume awal
        SetVolume(savedVolume);

        // Saat slider digeser
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float value)
    {
        // Ubah ke volume Unity (0-1)
        musicSource.volume = value / 100f;

        // Tampilkan persentase
        volumeText.text = value.ToString("0") + "%";

        // Simpan pengaturan
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
    }
}