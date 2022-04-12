using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SK_Stanicni_Racuni.Classes
{
    public class DirectoryAndFiles
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public DirectoryAndFiles(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }


        public bool IsDirectoryExist(string nazivStanice)
        {
            var uploadsFolder = $"{this.webHostEnvironment.WebRootPath}\\files\\" + nazivStanice;
            return Directory.Exists(uploadsFolder) == true ? true : false;
        }

        public bool IsFileExist(string fileName, string nazivStanice)
        {
            bool check = false;
            var path = $"{this.webHostEnvironment.WebRootPath}\\files\\" + nazivStanice;
            DirectoryInfo folderInfo = new DirectoryInfo(path);
            if (folderInfo.GetFiles().Length > 0)
            {
                foreach (FileInfo fileInfo in folderInfo.GetFiles())
                {
                    check = fileInfo.Name == fileName ? false : true;
                }
            }
            else
            {
                check = true;
            }

            return check;
        }

        public void CreateDirectory(string nazivStanice)
        {
            var uploadsFolder = $"{this.webHostEnvironment.WebRootPath}\\files\\" + nazivStanice;
            DirectoryInfo di = Directory.CreateDirectory(uploadsFolder);
        }

 
             

    }
}
