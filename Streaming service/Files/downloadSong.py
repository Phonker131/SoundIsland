try:
    from pydeezer.constants import track_formats
    from pydeezer import Deezer

    download_dir = 'Song Cache'
    with open("Song links.txt") as l:
        lines = l.readlines()
        list_of_url = [line.strip() for line in lines]

    with open("ARL.txt") as f:
        arl = f.readline()
        deezer = Deezer(arl=arl)
        user_info = deezer.user

    def get_track_id(list_of_url):
        for i in list_of_url:
            url = i.split("/")
            yield url[-1]

    def download(id_list):
        for i in id_list:
            track = deezer.get_track(i)
            track["download"](download_dir + f"\\{i}", quality=track_formats.MP3_320)

    id_list = get_track_id(list_of_url)
    download(id_list)
except Exception as ex:
    print(ex)

