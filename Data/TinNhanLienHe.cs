using System;
using System.Collections.Generic;

namespace MOBILUX.Data;

public partial class TinNhanLienHe
{
    public int Id { get; set; }

    public int MaLh { get; set; }

    public string NguoiGui { get; set; } = null!;

    public string NoiDung { get; set; } = null!;

    public DateTime ThoiGianGui { get; set; }
	
	public virtual LienHe LienHe { get; set; } = null!;

}
